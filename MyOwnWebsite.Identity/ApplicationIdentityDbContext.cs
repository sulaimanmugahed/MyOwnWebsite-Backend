
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyOwnWebsite.Identity.Models;

namespace MyOwnWebsite.Identity;

public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
: IdentityDbContext<AppUser, AppRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("Identity");

        builder.Entity<AppUser>(entity =>
        {
            entity.ToTable("Users");
        });

        builder.Entity<AppRole>(entity =>
        {
            entity.ToTable("Roles");
        });


        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles");
        });

        builder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        builder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("UserLogins");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("RoleClaims");

        });

        builder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("UserTokens");
        });
    }

}