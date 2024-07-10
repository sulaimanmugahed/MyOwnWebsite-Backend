using MediatR;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class GetAllProjectsPagedQuery : PaginationRequestParameter, IRequest<PagedResponse<ProjectDto>>
{
    public string? SearchValue { get; set; }
    public string? FilteredBy { get; set; }
}
