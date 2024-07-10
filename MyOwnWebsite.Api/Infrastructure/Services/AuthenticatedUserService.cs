using System.Security.Claims;
using MyOwnWebsite.Application.Contracts;

namespace MyOwnWebsite.Api.Infrastructure.Services;

public class AuthenticatedUserService(IHttpContextAccessor httpContext) : IAuthenticatedUserService
{
    public string UserId => httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public string UserName => httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.Name)!;
}

