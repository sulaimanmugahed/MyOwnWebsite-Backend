
using MediatR;
using Microsoft.AspNetCore.Authorization;
using MyOwnWebsite.Api.Infrastructure.Filters;
using MyOwnWebsite.Application.Features;

namespace MyOwnWebsite.Api.Endpoints;
public static class ProjectsEndpoint
{
    public static void AddProjectsEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        var projects = routeBuilder.MapGroup("/projects")
       .WithTags("projects")
       .WithOpenApi();

        projects.MapGet("/", async ([AsParameters] GetAllProjectsPagedQuery request, IMediator mediator)
        => await mediator.Send(request));

        projects.MapGet("/details/{id:guid}", [Authorize] async (Guid id, IMediator mediator)
        => await mediator.Send(new GetProjectDetailQuery { Id = id }));

        projects.MapPost("/", [Authorize(Roles = "Admin")] async (CreateProjectCommand command, IMediator Mediator)
        => await Mediator.Send(command)
        ).AddEndpointFilter<ValidationFilter<CreateProjectCommand>>();

        projects.MapDelete("/{id}", [Authorize(Roles = "Admin")] async (Guid id, IMediator mediator)
       => await mediator.Send(new DeleteProjectCommand { Id = id }));

        projects.MapPut("/{id}", [Authorize(Roles = "Admin")] async (Guid id, EditProjectCommand command, IMediator Mediator) =>
        {
            command.Id = id;
            return await Mediator.Send(command);
        }).AddEndpointFilter<ValidationFilter<EditProjectCommand>>();

        projects.MapPost("/{projectId:guid}/rate", [Authorize] async (Guid projectId, decimal rateValue, IMediator mediator)
        => await mediator.Send(new RateProjectCommand { ProjectId = projectId, RateValue = rateValue }));

        projects.MapGet("/{projectId:guid}/rate", [Authorize] async (Guid projectId, IMediator mediator)
        => await mediator.Send(new GetProjectRateByUserQuery { ProjectId = projectId }));

    }


}