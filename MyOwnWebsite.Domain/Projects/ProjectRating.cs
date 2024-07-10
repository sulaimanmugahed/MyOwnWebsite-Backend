
namespace MyOwnWebsite.Domain.Projects;

public class ProjectRating : Entity
{
    public Guid ProjectId { get; set; }

    public Guid RatingBy { get; set; }

    public decimal Value { get; set; }

    public DateTime RateAt { get; set; }
}

