using MediatR;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class RateProjectCommand : IRequest<BaseResult>
{
    public Guid ProjectId { get; set; }
    public decimal RateValue { get; set; }
}
