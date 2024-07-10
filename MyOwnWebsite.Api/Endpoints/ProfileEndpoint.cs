using MediatR;
using Microsoft.Extensions.Options;
using MyOwnWebsite.Application.Features;
using MyOwnWebsite.Application.Wrappers;
using MyOwnWebsite.Domain;

namespace MyOwnWebsite.Api.Endpoints;

public static class ProfileEndpoint
{
    public static void MapProfileEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        var profile = routeBuilder.MapGroup("profile")
        .WithTags("Profile")
       .WithOpenApi();

        profile.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetProfileDetailQuery());
        });


    }
}
