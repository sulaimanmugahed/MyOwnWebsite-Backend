
namespace MyOwnWebsite.Domain.Projects;


public class ProjectImage : Entity
{
    public Guid ProjectId { get; set; }
    public string Url { get; set; }
    public string FullPath  { get; set; }
    public bool IsHome { get; set; }

}