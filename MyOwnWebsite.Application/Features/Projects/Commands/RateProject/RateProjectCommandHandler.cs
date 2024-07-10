using MediatR;
using MyOwnWebsite.Application.Contracts;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Exceptions;
using MyOwnWebsite.Application.Wrappers;
using MyOwnWebsite.Domain.Projects;

namespace MyOwnWebsite.Application.Features;

public class RateProjectCommandHandler(
    IProjectRatingRepository projectRatingRepository,
    IAuthenticatedUserService authenticatedUserService,
    IUnitOfWork unitOfWork
    )
     : IRequestHandler<RateProjectCommand, BaseResult>
{
    public async Task<BaseResult> Handle(RateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = authenticatedUserService.UserId;
        if (userId is null)
        {
            throw new ApplicationNotFoundException("we could not get current user detail");
        }

        var newProjectRate = new ProjectRating
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            RatingBy = new Guid(userId),
            Value = request.RateValue,
            RateAt = DateTime.UtcNow
        };
        await projectRatingRepository.Rate(newProjectRate);
        await unitOfWork.SaveChangesAsync();

        return new BaseResult();
    }
}
