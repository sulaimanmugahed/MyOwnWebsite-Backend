namespace MyOwnWebsite.Domain.Profiles;

public class Profile : Entity
{

    public PersonalData PersonalData { get; set; }
    public List<Education> Educations { get; set; } = [];
    public List<Language> Languages { get; set; } = [];
    public List<Social> Socials { get; set; } = [];
    public List<Skill> Skills { get; set; } = [];
    public List<Experience> Experiences { get; set; } = [];
}
