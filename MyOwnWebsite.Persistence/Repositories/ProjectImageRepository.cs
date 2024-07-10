
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Domain.Projects;

namespace MyOwnWebsite.Persistence.Repositories;



public class ProjectImageRepository(ApplicationDbContext context) : Repository<ProjectImage>(context), IProjectImageRepository
{

}