using MediatR;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Exceptions;
using MyOwnWebsite.Application.Extensions;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class GetProjectDetailQueryHandler(
    IProjectRepository projectRepository
) : IRequestHandler<GetProjectDetailQuery, BaseResult<ProjectDetailsDto>>
{
    public async Task<BaseResult<ProjectDetailsDto>> Handle(GetProjectDetailQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetProjectAsync(request.Id);
        if (project is null)
        {
            throw new ApplicationNotFoundException("no project with this id", nameof(request.Id));

        }


        return new BaseResult<ProjectDetailsDto>(project?.AsDetailsDto()!);
    }
}
