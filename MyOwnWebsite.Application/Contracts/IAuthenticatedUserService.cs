namespace MyOwnWebsite.Application.Contracts;

public interface IAuthenticatedUserService
{
    string UserId { get; }
    string UserName { get; }
}