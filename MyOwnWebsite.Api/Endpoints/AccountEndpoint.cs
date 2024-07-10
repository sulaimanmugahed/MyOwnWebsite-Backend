using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOwnWebsite.Application.Contracts.Identity;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;
using MyOwnWebsite.Identity.Services;

namespace MyOwnWebsite.Api.Endpoints;


public static class AccountEndpoint
{
    public static void AddAccountEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        var account = routeBuilder.MapGroup("/account")
       .WithTags("account")
       .WithOpenApi();

        account.MapPost("googleAuth", async (HttpContext httpContext, ExternalTokenDTO externalToken, IAccountServices accountServices) =>
        {
            var result = await accountServices.GoogleAuth(externalToken);
            if (!result.Success)
                return Results.BadRequest();

            if (!string.IsNullOrEmpty(result.Data?.RefreshToken))
            {
                SetRefreshTokenToCookie(httpContext, result.Data.RefreshToken, result.Data.RefreshTokenExpiration);
            }

            return Results.Ok(result);
        });

        account.MapPost("githubAuth", async (HttpContext httpContext, ExternalTokenDTO externalToken, IAccountServices accountServices) =>
       {
           var result = await accountServices.GoogleAuth(externalToken);
           if (!result.Success)
               return Results.BadRequest();

           if (!string.IsNullOrEmpty(result.Data?.RefreshToken))
           {
               SetRefreshTokenToCookie(httpContext, result.Data.RefreshToken, result.Data.RefreshTokenExpiration);
           }

           return Results.Ok(result);
       });

        account.MapPost("login", async (HttpContext httpContext, AuthenticationRequest request, IAccountServices accountServices) =>
        {
            var result = await accountServices.Authenticate(request);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }
            if (!string.IsNullOrEmpty(result.Data?.RefreshToken))
            {
                SetRefreshTokenToCookie(httpContext, result.Data.RefreshToken, result.Data.RefreshTokenExpiration);
            }

            return Results.Ok(result);
        });



        account.MapGet("refreshToken", async (HttpContext httpContext, IAccountServices accountServices) =>
        {
            var refreshToken = httpContext.Request.Cookies["refreshToken"];
            if (refreshToken is null)
            {
                return Results.BadRequest();
            }
            var result = await accountServices.RefreshToken(refreshToken);

            if (!result.Success)
            {
                return Results.BadRequest(result);
            }

            SetRefreshTokenToCookie(httpContext, result.Data.RefreshToken, result.Data.RefreshTokenExpiration);
            return Results.Ok(result);
        });

        account.MapPost("register", async (HttpContext httpContext, RegistrationRequest request, IAccountServices accountServices) =>
        {
            var result = await accountServices.Register(request);
            if (!result.Success)
            {
                return Results.BadRequest(result);
            }
            // SetRefreshTokenToCookie(httpContext, result.Data.RefreshToken, result.Data.RefreshTokenExpiration);
            return Results.Ok(result);
        });

        account.MapGet("/confirm", async (string userEmail, string token, IAccountServices accountServices) =>
        {
            var result = await accountServices.ConfirmEmailAsync(userEmail, token);
            if (!result)
                return Results.BadRequest();

            return Results.Ok();
        });

        account.MapGet("resendConfirmToken", async (string email, IAccountServices accountServices) =>
        {
            var result = await accountServices.GenerateConfirmationTokenAndSendEmail(email);
            if (!result) return Results.BadRequest();

            return Results.Ok();
        });

        account.MapPost("revokeToken", async (HttpContext httpContext, [FromBody] RevokeToken request, IAccountServices accountServices) =>
       {
           var token = request.Token ?? httpContext.Request.Cookies["refreshToken"];
           if (string.IsNullOrEmpty(token))
           {
               return Results.BadRequest("token required");
           }

           var result = await accountServices.RevokeToken(token);
           if (!result)
               return Results.BadRequest("invalid token");

           return Results.Ok();
       });

        account.MapPost("signOut", async (HttpContext httpContext, IAccountServices accountServices) =>
        {
            await accountServices.SignOut();
            RemoveRefreshTokenFromCookie(httpContext);
            return Results.Ok(new BaseResult());
        });


        account.MapPost("isEmailExist", async (string email, IAccountServices accountServices) =>
          {
              return Results.Ok(await accountServices.IsEmailExist(email));
          });

        account.MapPost("isUsernameExist", async (string username, IAccountServices accountServices) =>
          {
              return Results.Ok(await accountServices.IsUsernameExist(username));
          });


        account.MapGet("test", SayHello);
    }

    [Authorize]
    private static string SayHello()
    {
        return "hello";
    }


    private static void SetRefreshTokenToCookie(HttpContext httpContext, string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime(),
            SameSite = SameSiteMode.Unspecified,
            Secure = false,
            IsEssential = true
        };

        httpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);


    }

    private static void RemoveRefreshTokenFromCookie(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete("refreshToken");
    }


}