using MediatR;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class GetAllProjectsPagedQueryHandler(
    IProjectRepository projectRepository
) : IRequestHandler<GetAllProjectsPagedQuery, PagedResponse<ProjectDto>>
{
    public async Task<PagedResponse<ProjectDto>> Handle(GetAllProjectsPagedQuery request, CancellationToken cancellationToken)
    {
        var result = await projectRepository.GetProjectPagedListAsync(request.PageNumber, request.PageSize, request.SearchValue, request.FilteredBy);
        return new PagedResponse<ProjectDto>(result, request);
    }
}
