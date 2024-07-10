

using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Domain.Projects;

namespace MyOwnWebsite.Application.Contracts.Persistence;

public interface IProjectRepository : IRepository<Project>
{
    Task<IReadOnlyCollection<Project>?> GetAllProjectsAsync();
    Task<Project?> GetProjectAsync(Guid id);
    void AddRange(List<Project> projects);
    Task<int> GetProjectsCount();
    Task<int> GetProjectsHasRatedCount();
    Task<PaginationResponseDto<ProjectDto>> GetProjectPagedListAsync(int pageNumber, int pageSize, string? searchValue, string? FilteredBy);
}