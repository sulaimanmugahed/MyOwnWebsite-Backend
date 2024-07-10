namespace MyOwnWebsite.Application.Dtos;

public class AllProjectRatingStatistic
{
    public int TotalNumberOfRates { get; set; }
    public int TotalProjectsCount { get; set; }
    public int TotalProjectsHasRatedCount { get; set; }

    public string? TopProjectRate { get; set; }

    public List<NumberOfRate> NumberOfRates { get; set; } = [];

}

public class NumberOfRate
{
    public int Rating { get; set; }
    public int Count { get; set; }
}


