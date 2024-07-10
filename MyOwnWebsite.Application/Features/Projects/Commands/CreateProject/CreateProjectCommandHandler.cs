using MediatR;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Extensions;
using MyOwnWebsite.Application.Wrappers;
using MyOwnWebsite.Domain.Projects;

namespace MyOwnWebsite.Application.Features;

public class CreateProjectCommandHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateProjectCommand, BaseResult<string>>
{
    public async Task<BaseResult<string>> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Description = command.Description,
            GithubLink = command.GithubLink,
            StartDate = command.StartDate.SetKindUtc(),
            EndDate = command.EndDate.SetKindUtc(),
            Created = DateTime.UtcNow
        };

        if (command.HomeImage is not null)
        {
            project.AddImage(command.HomeImage.Url, command.HomeImage.FullPath, true);
        }


        if (command.AdditionalImages?.Count > 0)
        {
            foreach (var image in command.AdditionalImages)
            {
                project.AddImage(image.Url, image.FullPath);
            }
        }


        if (command.Features?.Count > 0)
        {
            foreach (var feature in command.Features)
            {
                project.AddFeature(feature);
            }
        }

        await projectRepository.AddAsync(project);
        await unitOfWork.SaveChangesAsync();

        return new BaseResult<string>(project.Id.ToString());
    }
}
