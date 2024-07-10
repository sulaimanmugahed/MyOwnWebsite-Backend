using MediatR;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class GetAllProjectRatingStatisticQuery:IRequest<BaseResult<AllProjectRatingStatistic>>
{

}
