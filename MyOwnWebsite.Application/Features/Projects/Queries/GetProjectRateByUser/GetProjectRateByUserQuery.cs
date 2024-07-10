using MediatR;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class GetProjectRateByUserQuery:IRequest<BaseResult<UserRateDto?>>
{
    public Guid ProjectId { get; set; }
}
