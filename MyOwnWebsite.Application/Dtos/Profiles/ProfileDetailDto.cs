using MyOwnWebsite.Domain.Profiles;

namespace MyOwnWebsite.Application.Dtos;

public record ProfileDetailDto(
    PersonalData PersonalData,
    List<Education> Educations,
    List<Experience> Experiences,
    List<Language> Languages,
    List<Social> Socials,
    List<SkillsGroupDto> SkillsGroups);

