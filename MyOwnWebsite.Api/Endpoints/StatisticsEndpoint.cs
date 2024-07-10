using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using MyOwnWebsite.Application.Features;

namespace MyOwnWebsite.Api.Endpoints;
public static class StatisticsEndpoint
{
    public static void AddStatisticsEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        var statistics = routeBuilder.MapGroup("statistics")
        .WithTags("Statistic")
       .WithOpenApi();


        statistics.MapGet("/allProjectRatingStatistic", [Authorize(Roles = "Admin")] async (IMediator mediator)
        => await mediator.Send(new GetAllProjectRatingStatisticQuery()));

    }
}