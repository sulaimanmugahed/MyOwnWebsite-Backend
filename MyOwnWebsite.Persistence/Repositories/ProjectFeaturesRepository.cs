using Microsoft.EntityFrameworkCore;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Domain.Projects;

namespace MyOwnWebsite.Persistence.Repositories;

public class ProjectFeaturesRepository(ApplicationDbContext context): Repository<ProjectFeature>(context),IProjectFeaturesRepository
{
    private readonly DbSet<ProjectFeature> projectFeatures = context.Set<ProjectFeature>();
}