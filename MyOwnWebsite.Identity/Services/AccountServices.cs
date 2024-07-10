using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyOwnWebsite.Application.Contracts;
using MyOwnWebsite.Application.Contracts.Identity;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;
using MyOwnWebsite.Domain.Settings;
using MyOwnWebsite.Identity.Extensions;
using MyOwnWebsite.Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MyOwnWebsite.Identity.Services;

public class AccountServices(UserManager<AppUser> userManager, IEmailService emailService, IAuthenticatedUserService authenticatedUser, SignInManager<AppUser> signInManager, IOptionsSnapshot<JWTSettings> jwtSettings, IOptions<CorsSettings> corsSettings) : IAccountServices
{

    public async Task<BaseResult<AuthenticationResponse>> GoogleAuth(ExternalTokenDTO externalToken)
    {
        var email = ValidateToken.Authenticate(externalToken.Token);

        if (email is not null)
        {
            var existUser = await userManager.FindByEmailAsync(email);

            if (existUser is null)
            {

                var user = new AppUser
                {
                    Name = email,
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user);
                var token = await GenerateJwtToken(user);
                if (result.Succeeded)
                {
                    if (!await userManager.IsInRoleAsync(user, "User"))
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }


                    AuthenticationResponse responseR = new AuthenticationResponse()
                    {

                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        Id = user.Id.ToString(),
                        Email = user.Email,
                        Name = user.Name,
                        Roles = ["User"],
                        IsVerified = user.EmailConfirmed,
                    };

                    return new BaseResult<AuthenticationResponse>(responseR);
                }

                return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.FieldDataInvalid, "Not found"));

            }


            var rolesList = await userManager.GetRolesAsync(existUser).ConfigureAwait(false);

            var jwToken = await GenerateJwtToken(existUser);


            AuthenticationResponse response = new AuthenticationResponse()
            {
                Id = existUser.Id.ToString(),
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwToken),
                Email = existUser.Email,
                Name = existUser.Name,
                Roles = rolesList.ToList(),
                IsVerified = existUser.EmailConfirmed,

            };

            if (existUser.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = existUser.RefreshTokens.FirstOrDefault(t => t.IsActive);
                response.RefreshToken = activeRefreshToken.Token;
                response.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                response.RefreshToken = refreshToken.Token;
                response.RefreshTokenExpiration = refreshToken.ExpiresOn;

                existUser.RefreshTokens.Add(refreshToken);
                await userManager.UpdateAsync(existUser);

            }

            return new BaseResult<AuthenticationResponse>(response);
        }


        return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.FieldDataInvalid, "Not found"));

    }

    public async Task<bool> GenerateConfirmationTokenAndSendEmail(string userEmail)
    {
        var user = await userManager.FindByEmailAsync(userEmail);
        if (user is null) return false;

        var token = await GenerateConfirmationToken(user);
        await SendConfirmationEmail(userEmail, token, user.Name);
        return true;
    }

    private async Task SendConfirmationEmail(string email, string token, string name)
    {
        var confirmationLink = $"{corsSettings.Value.AllowedOrigin}/verifyEmail?userEmail={email}&token={token}";

        string htmlContent = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Welcome to My Website</title>
  <style>
    body {{
      font-family: sans-serif;
      margin: 0;
      padding: 0;
      background-color: #f5f5f5;
    }}
    .container {{
      max-width: 600px;
      margin: 50px auto;
      background-color: #fff;
      border-radius: 5px;
      padding: 30px;
      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    }}
    .header {{
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }}
    .logo {{
      width: 100px;
      height: auto;
    }}
    .content {{
      font-size: 16px;
      line-height: 1.5;
    }}
    .cta {{
      display: inline-block;
      text-decoration: none;
      padding: 10px 20px;
      border-radius: 5px;
      color: #fff;
      background-color: #8DA290;
      font-weight: bold;
      margin-top: 20px;
    }}
    a{{
        color:#8DA290;
    }}
    .footer {{
      text-align: center;
      font-size: 12px;
      color: #aaa;
      margin-top: 20px;
    }}
  </style>
</head>
<body>
  <div class=""container"">
    <div class=""header"">
      <h2>Welcome!</h2>
    </div>
    <div class=""content"">
      <p>Hi {name},</p>
      <p>Thanks for signing up in My Website!</p>
      <p>To confirm your email address and start using your account, please click the button below:</p>
      <a href=""{confirmationLink}"" class=""cta"">Confirm Email Address</a>
      <p>If you can't click the button, you can also paste the following link into your browser:</p>
      <p>{confirmationLink}</p>
      <p>**Please note:** This link will expire in 24 hours.</p>
    </div>
    <div class=""footer"">
      <p>&copy; {DateTime.Now.Year} Sulaiman Mugahed. All rights reserved.</p>
    </div>
  </div>
</body>
</html>";



        await emailService.SendEmail(email, "Confirm Your Email", htmlContent);
    }

    private async Task<string> GenerateConfirmationToken(AppUser user)
    {
        return await userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<bool> ConfirmEmailAsync(string userEmail, string token)
    {
        var user = await userManager.FindByEmailAsync(userEmail);

        if (user is null) return false;

        token = token.Replace(" ", "+");
        var result = await userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded;
    }





    public async Task<BaseResult> Register(RegistrationRequest request)
    {
        var user = new AppUser
        {
            Name = request.Name,
            Email = request.Email,
            UserName = request.Username

        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return new BaseResult(result.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)));


        var roleResult = await userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded)
            return new BaseResult(roleResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)));

        var token = await GenerateConfirmationToken(user);
        await SendConfirmationEmail(user.Email, token, user.Name);

        return new BaseResult();
    }


    public async Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
    {

        var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName || u.Email == request.UserName);
        if (user == null)
        {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.NotFound, "Not found", nameof(request.UserName)));
        }
        var result = await signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.FieldDataInvalid, "Not found", nameof(request.Password)));
        }

        var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);

        var jwToken = await GenerateJwtToken(user);


        AuthenticationResponse response = new AuthenticationResponse()
        {
            Id = user.Id.ToString(),
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwToken),
            Email = user.Email,
            Name = user.Name,
            Roles = rolesList.ToList(),
            IsVerified = user.EmailConfirmed,

        };

        if (user.RefreshTokens.Any(t => t.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            response.RefreshToken = activeRefreshToken.Token;
            response.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;

        }
        else
        {
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;
            response.RefreshTokenExpiration = refreshToken.ExpiresOn;

            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);

        }


        return new BaseResult<AuthenticationResponse>(response);
    }


    public async Task<BaseResult<bool>> IsEmailExist(string email)
    {
        var result = await userManager.Users.AnyAsync(u => u.Email == email);
        return new BaseResult<bool>(result);
    }

    public async Task<BaseResult<bool>> IsUsernameExist(string username)
    {
        var result = await userManager.Users.AnyAsync(u => u.UserName == username);
        return new BaseResult<bool>(result);
    }



    private async Task<JwtSecurityToken> GenerateJwtToken(AppUser user)
    {
        await userManager.UpdateSecurityStampAsync(user);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: jwtSettings.Value.Issuer,
            audience: jwtSettings.Value.Audience,
            claims: await GetClaimsAsync(),
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.Value.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;

        async Task<IList<Claim>> GetClaimsAsync()
        {
            var result = await signInManager.ClaimsFactory.CreateAsync(user);
            return result.Claims.ToList();
        }
    }

    public async Task<BaseResult<AuthenticationResponse>> RefreshToken(string token)
    {


        var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
        {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.ErrorInIdentity, "invalid token", nameof(token)));
        }

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
        if (!refreshToken.IsActive)
        {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.ErrorInIdentity, "InActiveToken", nameof(token)));
        }

        refreshToken.RevokedOn = DateTime.UtcNow;
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await userManager.UpdateAsync(user);

        var jwtToken = await GenerateJwtToken(user);

        var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);


        AuthenticationResponse response = new AuthenticationResponse()
        {
            Id = user.Id.ToString(),
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            Email = user.Email,
            Name = user.Name,
            Roles = rolesList.ToList(),
            IsVerified = user.EmailConfirmed,

        };

        response.RefreshToken = newRefreshToken.Token;
        response.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

        return new BaseResult<AuthenticationResponse>(response);

    }

    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(30),
            CreatedOn = DateTime.UtcNow
        };

    }

    public async Task<bool> RevokeToken(string token)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user is null)
            return false;

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
        if (!refreshToken.IsActive)
            return false;


        refreshToken.RevokedOn = DateTime.UtcNow;

        await userManager.UpdateAsync(user);
        return true;

    }

    public async Task SignOut()
    {
        await signInManager.SignOutAsync();
    }

}
