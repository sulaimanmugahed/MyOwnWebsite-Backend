using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Domain.Projects;


namespace MyOwnWebsite.Application.Contracts.Persistence;
public interface IProjectRatingRepository
{
    Task Rate(ProjectRating projectRating);
    Task<UserRateDto?> GetUserRate(Guid userId, Guid projectId);
    Task<Project?> GetProjectWithMaxRating();
    Task<int> GetTotalNumberOfRates();
    Task<Dictionary<int, int>> GetNumberOfRates();
}