

using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Application.Contracts.Identity;

public interface IAccountServices
{
    Task<BaseResult<AuthenticationResponse>> GoogleAuth(ExternalTokenDTO externalToken);
    Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request);
    Task<BaseResult> Register(RegistrationRequest request);
    Task<bool> GenerateConfirmationTokenAndSendEmail(string userEmail);
    Task<bool> ConfirmEmailAsync(string userEmail, string token);
    Task<BaseResult<AuthenticationResponse>> RefreshToken(string token);
    Task<bool> RevokeToken(string token);
    Task SignOut();
    Task<BaseResult<bool>> IsEmailExist(string email);
    Task<BaseResult<bool>> IsUsernameExist(string username);

}