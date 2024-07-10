using Microsoft.AspNetCore.Identity;

namespace MyOwnWebsite.Identity.Models;


public class AppUser : IdentityUser<Guid>
{
    public string Name { get; set; }
    public List<RefreshToken>? RefreshTokens { get; set; }
}