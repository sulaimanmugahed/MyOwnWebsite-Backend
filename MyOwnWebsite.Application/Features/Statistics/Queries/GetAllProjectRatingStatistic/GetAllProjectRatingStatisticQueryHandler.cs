using MediatR;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class GetAllProjectRatingStatisticQueryHandler(
    IProjectRatingRepository projectRatingRepository,
    IProjectRepository projectRepository
) : IRequestHandler<GetAllProjectRatingStatisticQuery, BaseResult<AllProjectRatingStatistic>>
{
    public async Task<BaseResult<AllProjectRatingStatistic>> Handle(GetAllProjectRatingStatisticQuery request, CancellationToken cancellationToken)
    {
        var numbersOfRates = await projectRatingRepository.GetNumberOfRates();

        AllProjectRatingStatistic result = new()
        {
            TotalNumberOfRates = await projectRatingRepository.GetTotalNumberOfRates(),
            TotalProjectsCount = await projectRepository.GetProjectsCount(),
            TotalProjectsHasRatedCount = await projectRepository.GetProjectsHasRatedCount(),
            TopProjectRate = (await projectRatingRepository.GetProjectWithMaxRating())?.Title,
            NumberOfRates = numbersOfRates.Select(x =>
             new NumberOfRate
             {
                 Rating = x.Key,
                 Count = x.Value
             }).ToList()
        };

        return new BaseResult<AllProjectRatingStatistic>(result);
    }
}
