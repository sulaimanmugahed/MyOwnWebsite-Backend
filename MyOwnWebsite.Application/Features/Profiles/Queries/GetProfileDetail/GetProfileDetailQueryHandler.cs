using MediatR;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Extensions;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Features;

public class GetProfileDetailQueryHandler(
    IProfileRepository profileRepository
) : IRequestHandler<GetProfileDetailQuery, BaseResult<ProfileDetailDto?>>
{
    public async Task<BaseResult<ProfileDetailDto?>> Handle(GetProfileDetailQuery request, CancellationToken cancellationToken)
    {
        var result = await profileRepository.GetAsync(p => p.PersonalData.FirstName != null);
        return new BaseResult<ProfileDetailDto?>(result?.AsDetailDto());
    }
}
