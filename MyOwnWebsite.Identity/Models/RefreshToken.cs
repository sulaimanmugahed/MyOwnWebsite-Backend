using Microsoft.EntityFrameworkCore;

namespace MyOwnWebsite.Identity.Models;

[Owned]
public class RefreshToken
{
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    public DateTime CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; }
    public bool IsActive => RevokedOn is null && !IsExpired;

}