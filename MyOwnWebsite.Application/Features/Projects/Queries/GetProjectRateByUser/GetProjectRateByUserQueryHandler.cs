using MediatR;
using MyOwnWebsite.Application.Contracts;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Exceptions;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class GetProjectRateByUserQueryHandler
(
   IProjectRatingRepository projectRatingRepository,
   IAuthenticatedUserService authenticatedUserService
) : IRequestHandler<GetProjectRateByUserQuery, BaseResult<UserRateDto?>>
{
    public async Task<BaseResult<UserRateDto?>> Handle(GetProjectRateByUserQuery request, CancellationToken cancellationToken)
    {
        var userId = authenticatedUserService.UserId;
        if (userId is null)
        {
            throw new ApplicationNotFoundException("we could not get current user detail");

        }
        var result = await projectRatingRepository.GetUserRate(Guid.Parse(userId), request.ProjectId);
        return new BaseResult<UserRateDto?>(result);
    }
}
