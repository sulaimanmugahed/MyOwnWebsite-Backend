using MediatR;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class CreateProjectCommand : IRequest<BaseResult<string>>, IProjectCommand
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public List<string>? Features { get; set; }
    public string? GithubLink { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectImageDto? HomeImage { get; set; }
    public List<ProjectImageDto>? AdditionalImages { get; set; }
}
