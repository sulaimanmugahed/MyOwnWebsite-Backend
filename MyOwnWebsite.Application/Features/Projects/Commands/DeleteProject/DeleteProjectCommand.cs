using MediatR;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class DeleteProjectCommand : IRequest<BaseResult>
{
    public Guid Id { get; set; }
}
