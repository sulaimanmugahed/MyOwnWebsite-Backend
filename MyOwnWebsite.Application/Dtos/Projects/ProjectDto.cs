namespace MyOwnWebsite.Application.Dtos;

public record ProjectDto(Guid Id, string Title, string Description, string? HomeImage, decimal TotalRate, DateTime StartDate);
public record ProjectDetailsDto(Guid Id, string Title,
 string Description,
  List<string>? Features,
   List<ProjectImageDto>? AdditionalImages,
    ProjectImageDto? HomeImage,
     List<RatingAverageDto> RatingAverages,
     int TotalRateCount, DateTime Created, decimal TotalRateAverage, string? GithubLink, DateTime StartDate, DateTime? EndDate);


public record RateAverageDto(List<decimal> RatingsAverage);

public class ProjectCommand
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string>? Features { get; set; }
    public string? GithubLink { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectImageDto? HomeImage { get; set; }

    public List<ProjectImageDto>? AdditionalImages { get; set; }
};


public record UserRateDto(decimal Rate, DateTime RateAt);

