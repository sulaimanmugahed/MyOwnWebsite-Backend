

using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Domain.Projects;

namespace MyOwnWebsite.Application.Extensions;

public static class ProjectMappingExtension
{
  public static ProjectDto AsDto(this Project project)
  => new(
   project.Id,
   project.Title,
   project.Description,
   project.Images.FirstOrDefault(i => i.IsHome)?.Url,
   project.CalculateTotalRate(),
   project.StartDate
  );

  public static ProjectDetailsDto AsDetailsDto(this Project project)
  {
    var homeImage = project.Images.FirstOrDefault(i => i.IsHome);
    ProjectImageDto homeImageDto = null;
    if (homeImage is not null)
    {
      homeImageDto = new ProjectImageDto { Url = homeImage.Url, FullPath = homeImage.FullPath };
    }
    return new(
     project.Id,
     project.Title,
     project.Description,
     project.Features.Select(f => f.Text).ToList(),
     project.Images.Where(i => !i.IsHome).Select(i => new ProjectImageDto { Url = i.Url, FullPath = i.FullPath }).ToList(),
    homeImageDto,
     project.Ratings.GroupBy(r => r.Value)
            .Select(g => new RatingAverageDto
            {
              Rate = (int)g.Key,
              Average = (int)((double)g.Count() / project.Ratings.Count * 100)
            }).ToList(),
     project.Ratings.Count,
     project.Created,
     project.CalculateTotalRate(),
     project.GithubLink,
     project.StartDate,
     project.EndDate
    );
  }

}

