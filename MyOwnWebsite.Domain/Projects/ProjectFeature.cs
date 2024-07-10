
namespace MyOwnWebsite.Domain.Projects;


public class ProjectFeature : Entity
{
    public Project Project { get; set; }
    public Guid ProjectId { get; set; }
    public string Text { get; set; }
}