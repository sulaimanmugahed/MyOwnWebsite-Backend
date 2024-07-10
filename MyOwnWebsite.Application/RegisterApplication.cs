using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace MyOwnWebsite.Application;

public static class RegisterApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(opt => opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
