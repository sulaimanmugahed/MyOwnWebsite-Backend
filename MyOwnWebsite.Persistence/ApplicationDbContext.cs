using Microsoft.EntityFrameworkCore;
using MyOwnWebsite.Domain;
using MyOwnWebsite.Domain.Profiles;
using MyOwnWebsite.Domain.Projects;


namespace MyOwnWebsite.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ProjectRating> ProjectRatings { get; set; }
    public DbSet<ProjectFeature> ProjectFeatures { get; set; }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {

        return base.SaveChangesAsync(cancellationToken);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Profile>(builder =>
        {
            builder.ToTable("Profiles");
            builder.Property(pf => pf.Version)
              .IsRowVersion();
            builder.OwnsMany(
             p => p.Skills, skill =>
            {
                skill.OwnsOne(s => s.Type);
            });

            builder.OwnsOne(p=> p.PersonalData);
            builder.OwnsMany(p => p.Educations);
            builder.OwnsMany(p => p.Experiences);
            builder.OwnsMany(p => p.Socials);
            builder.OwnsMany(p => p.Languages);

        });

        modelBuilder.Entity<Project>(builder =>
        {
            builder.ToTable("Projects", "Main");
            builder.Property(pf => pf.Version)
               .IsRowVersion();

        });


        modelBuilder.Entity<ProjectFeature>(builder =>
        {
            builder.ToTable("ProjectFeatures", "Main");
            builder.HasOne(x => x.Project)
            .WithMany(p => p.Features)
            .HasForeignKey(pf => pf.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pf => pf.Version)
               .IsRowVersion();

        });

        ////

        modelBuilder.Entity<ProjectRating>(builder =>
       {
           builder.ToTable("ProjectRatings", "Main");
           builder.HasOne<Project>()
           .WithMany(p => p.Ratings)
           .HasForeignKey(pf => pf.ProjectId);
           builder.Property(pf => pf.Version)
               .IsRowVersion();
       });

        ////

        modelBuilder.Entity<ProjectImage>(builder =>
             {
                 builder.ToTable("ProjectImages", "Main");
                 builder.HasOne<Project>()
                 .WithMany(p => p.Images)
                 .HasForeignKey(pf => pf.ProjectId);
                 builder.Property(pf => pf.Version)
                 .IsRowVersion();
             });




    }
}