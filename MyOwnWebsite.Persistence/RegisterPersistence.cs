using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Persistence.Repositories;

namespace MyOwnWebsite.Persistence;

public static class RegisterPersistence
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DbConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<IProjectRepository, ProjectRepository>();
        services.AddTransient<IProfileRepository, ProfileRepository>();
        services.AddTransient<IProjectRatingRepository, ProjectRatingRepository>();
        services.AddTransient<IProjectFeaturesRepository, ProjectFeaturesRepository>();
        services.AddTransient<IProjectImageRepository, ProjectImageRepository>();

        return services;
    }
}