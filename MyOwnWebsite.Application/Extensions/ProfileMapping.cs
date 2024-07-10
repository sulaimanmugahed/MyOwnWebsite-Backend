using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Domain.Profiles;

namespace MyOwnWebsite.Application.Extensions;


public static class ProfileMapping
{
    public static ProfileDetailDto AsDetailDto(this Profile profile)
    => new(
        profile.PersonalData,
        profile.Educations,
        profile.Experiences,
        profile.Languages,
        profile.Socials,
        profile.Skills
        .GroupBy(s => s.Type.Value)
        .Select(g => new SkillsGroupDto
        {
            Type = g.Key,
            Skills = g.Select(s => s.Value).ToList()
        }).ToList()
    );
}
