using Microsoft.EntityFrameworkCore;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Domain.Projects;


namespace MyOwnWebsite.Persistence.Repositories;


public class ProjectRatingRepository(ApplicationDbContext context) : IProjectRatingRepository
{
    public async Task Rate(ProjectRating projectRating)
    {
        var rate = await context.ProjectRatings.FirstOrDefaultAsync(x => x.RatingBy == projectRating.RatingBy && x.ProjectId == projectRating.ProjectId);
        if (rate is null)
        {
            await context.ProjectRatings.AddAsync(projectRating);
            return;
        }

        rate.Value = projectRating.Value;
        rate.RateAt = DateTime.UtcNow;
    }

    public async Task<UserRateDto?> GetUserRate(Guid userId, Guid projectId)
    {
        var result = await context.ProjectRatings.FirstOrDefaultAsync(r => r.RatingBy == userId && r.ProjectId == projectId);
        if (result is null)
            return null;

        return new UserRateDto(result.Value, result.RateAt);
    }

    public async Task<Project?> GetProjectWithMaxRating()
    {
        return await context.Projects.OrderByDescending(x => x.Ratings.Count).FirstOrDefaultAsync();
    }

    public async Task<int> GetTotalNumberOfRates() => await context.ProjectRatings.CountAsync();


    public async Task<Dictionary<int, int>> GetNumberOfRates()
    {
        return context.ProjectRatings.GroupBy(r => r.Value).ToDictionary(g => (int)g.Key, g => g.Count());
    }





}