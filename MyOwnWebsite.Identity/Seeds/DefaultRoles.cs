using Microsoft.AspNetCore.Identity;
using MyOwnWebsite.Identity.Models;


namespace MyOwnWebsite.Identity.Seeds;


public static class DefaultRoles
{
    public static async Task SeedAsync(RoleManager<AppRole> roleManager)
    {
        //Seed Roles
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new AppRole("Admin"));


        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new AppRole("User"));
    }
}

