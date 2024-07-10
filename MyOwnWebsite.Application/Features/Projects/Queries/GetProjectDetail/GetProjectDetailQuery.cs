using MediatR;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class GetProjectDetailQuery : IRequest<BaseResult<ProjectDetailsDto>>
{
    public Guid Id { get; set; }
}
