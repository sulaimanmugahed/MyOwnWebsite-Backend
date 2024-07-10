

using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Identity.Models;

namespace MyOwnWebsite.Identity.Extensions;

public static class UserMappingExtension
{
    public static UserDto AsDto(this AppUser user)
    => new(
        user.Id,
        user.Name,
        user.Email
    );
}