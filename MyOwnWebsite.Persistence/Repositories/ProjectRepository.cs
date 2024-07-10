using Microsoft.EntityFrameworkCore;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Domain.Projects;
using MyOwnWebsite.Application.Extensions;


namespace MyOwnWebsite.Persistence.Repositories;

public class ProjectRepository(ApplicationDbContext context) : Repository<Project>(context), IProjectRepository
{
    private readonly DbSet<Project> projects = context.Set<Project>();


    public async Task<IReadOnlyCollection<Project>?> GetAllProjectsAsync()
    => await projects
    .Include(p => p.Features)
    .Include(p => p.Images)
    .Include(p => p.Ratings)
    .ToListAsync();


    public async Task<Project?> GetProjectAsync(Guid id)
   => await projects
   .Include(p => p.Features)
   .Include(p => p.Images)
   .Include(p => p.Ratings)
   .FirstOrDefaultAsync(x => x.Id == id);



    public void AddRange(List<Project> projects)
    {
        projects.AddRange(projects);

    }

    public async Task<int> GetProjectsCount() => await projects.CountAsync();
    public async Task<int> GetProjectsHasRatedCount() => await projects.Include(x => x.Ratings).Where(p => p.Ratings.Count > 0).CountAsync();


    public async Task<PaginationResponseDto<ProjectDto>> GetProjectPagedListAsync(int pageNumber, int pageSize, string? searchValue, string? filteredBy)
    {
        var query = projects
        .Include(p => p.Images)
        .Include(p => p.Ratings)
        .OrderByDescending(p => p.StartDate)
        .AsQueryable();


        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(p => p.Title.Contains(searchValue));
        }

        if (!string.IsNullOrEmpty(filteredBy))
        {
            switch (filteredBy)
            {
                case "oldest":
                    query = query.OrderBy(p => p.StartDate);
                    break;

                case "topRated":
                    query = query.OrderByDescending(p => p.Ratings.Sum(rate => rate.Value));
                    break;


                default: break;
            }
        }

        return await Paged(
            query.Select(p => p.AsDto()),
            pageNumber,
            pageSize);

    }

}