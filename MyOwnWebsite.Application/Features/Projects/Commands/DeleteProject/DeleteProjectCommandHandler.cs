using MediatR;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Exceptions;
using MyOwnWebsite.Application.Features;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application;

public class DeleteProjectCommandHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork
    )
    : IRequestHandler<DeleteProjectCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetProjectAsync(request.Id);
        if (project is null)
        {

            throw new ApplicationNotFoundException("no project with this id", nameof(request.Id));
        }

        await projectRepository.RemoveAsync(project);

        await unitOfWork.SaveChangesAsync();
        return new BaseResult();
    }
}
