
namespace MyOwnWebsite.Domain.Projects;


public class Project : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<ProjectImage> Images { get; set; } = [];
    public List<ProjectRating> Ratings { get; set; } = [];
    public List<ProjectFeature> Features { get; set; } = [];
    public string? GithubLink { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime Created { get; set; }
    public decimal CalculateTotalRate()
    {
        var ratingCounts = Ratings.Count;
        return ratingCounts == 0
        ? 0
        : Math.Round(Ratings.Select(x => x.Value).Sum() / ratingCounts, 1);
    }


    public void AddImage(string url, string fullPath, bool isHome = false)
    {
        var image = new ProjectImage
        {
            Id = Guid.NewGuid(),
            ProjectId = Id,
            Url = url,
            FullPath = fullPath,
            IsHome = isHome

        };

        Images.Add(image);
    }

    public void AddFeature(string text)
    {

        var feature = new ProjectFeature
        {
            Id = Guid.NewGuid(),
            Project = this,
            Text = text
        };

        Features.Add(feature);
    }

    public void UpdateFeatures(List<string> features)
    {
        var featuresToRemove = Features
                .Where(f => !features.Contains(f.Text))
                .ToList();

        if (featuresToRemove.Count > 0)
            Features.RemoveAll(featuresToRemove.Contains);

        var featuresToAdd = features
       .Except(Features.Select(f => f.Text)).ToList();

        if (featuresToAdd.Count > 0)
        {
            foreach (var feature in featuresToAdd)
            {
                AddFeature(feature);
            }
        }


    }

}