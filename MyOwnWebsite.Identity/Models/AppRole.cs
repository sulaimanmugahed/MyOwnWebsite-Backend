using Microsoft.AspNetCore.Identity;

namespace MyOwnWebsite.Identity.Models;


public class AppRole(string name) : IdentityRole<Guid>(name)
{

}