
using System.IdentityModel.Tokens.Jwt;

namespace MyOwnWebsite.Identity.Extensions;

public static class ValidateToken
{
    public static string? Authenticate(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        var _token = jsonToken as JwtSecurityToken;
        var iss = _token?.Claims.First(claim => claim.Type == "iss").Value;

        if (iss == "https://securetoken.google.com/sulaiman-mugahed")
        {
            var email = _token?.Claims.First(claim => claim.Type == "email").Value;
            return email;
        }


        return null;
    }
}