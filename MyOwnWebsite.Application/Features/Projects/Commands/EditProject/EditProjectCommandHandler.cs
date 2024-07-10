using MediatR;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Exceptions;
using MyOwnWebsite.Application.Extensions;
using MyOwnWebsite.Application.Wrappers;
using MyOwnWebsite.Domain.Projects;

namespace MyOwnWebsite.Application.Features;

public class EditProjectCommandHandler(
    IProjectRepository projectRepository,
    IProjectFeaturesRepository projectFeaturesRepository,
    IProjectImageRepository projectImageRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<EditProjectCommand, BaseResult>
{
    public async Task<BaseResult> Handle(EditProjectCommand command, CancellationToken cancellationToken)
    {
         var project = await projectRepository.GetProjectAsync(command.Id);
        if (project is null)
        {
            throw new ApplicationNotFoundException( "no project with this id", nameof(command.Id));
           
        }

        project.Title = command.Title;
        project.Description = command.Description;
        project.GithubLink = command.GithubLink;
        project.StartDate = command.StartDate.SetKindUtc();
        project.EndDate = command.EndDate.SetKindUtc();

        if (command.Features is not null)
        {
            // project.UpdateFeatures(command.Features);
            var featuresToRemove = project.Features
                .Where(f => !command.Features.Contains(f.Text))
                .ToList();

            if (featuresToRemove.Count > 0)
            {
                foreach (var feature in featuresToRemove)
                {
                    await projectFeaturesRepository.RemoveAsync(feature);
                }
            }

            var featuresToAdd = command.Features
           .Except(project.Features.Select(f => f.Text)).ToList();

            if (featuresToAdd.Count > 0)
            {
                foreach (var feature in featuresToAdd)
                {
                    var featureToAdd = new ProjectFeature
                    {
                        Id = Guid.NewGuid(),
                        Text = feature,
                        ProjectId = project.Id
                    };
                    await projectFeaturesRepository.AddAsync(featureToAdd);
                }
            }

        }

        if (command.HomeImage is not null)
        {
            var existHomeImage = await projectImageRepository.GetAsync(i => i.IsHome && i.ProjectId == project.Id);
            if (existHomeImage is null)
            {
                await projectImageRepository.AddAsync(new ProjectImage
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project.Id,
                    Url = command.HomeImage.Url,
                    FullPath = command.HomeImage.FullPath,
                    IsHome = true
                });
            }
            else
            {
                existHomeImage.FullPath = command.HomeImage.Url;
                existHomeImage.Url = command.HomeImage.Url;
            }
        }


        if (command.AdditionalImages is not null)
        {
            var imagesToRemove = project.Images
            .Where(image => !command.AdditionalImages
            .Select(x => x.Url).Contains(image.Url) && !image.IsHome)
            .ToList();

            if (imagesToRemove is not null && imagesToRemove.Count > 0)
            {
                foreach (var image in imagesToRemove)
                {
                    await projectImageRepository.RemoveAsync(image);
                }
            }

            var imagesToAdd = command.AdditionalImages
            .Where(image => !project.Images.Select(i => i.Url).Contains(image.Url))
            .ToList();

            if (imagesToAdd is not null && imagesToAdd.Count > 0)
            {
                foreach (var image in imagesToAdd)
                {
                    await projectImageRepository.AddAsync(new ProjectImage
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = project.Id,
                        Url = image.Url,
                        FullPath = image.FullPath
                    });
                }
            }


        }

        await unitOfWork.SaveChangesAsync();

        return new BaseResult();
    }
}
