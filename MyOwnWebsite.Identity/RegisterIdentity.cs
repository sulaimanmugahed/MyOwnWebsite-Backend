using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyOwnWebsite.Application.Contracts.Identity;
using MyOwnWebsite.Application.Wrappers;
using MyOwnWebsite.Domain.Settings;
using MyOwnWebsite.Identity.Models;
using MyOwnWebsite.Identity.Services;


namespace MyOwnWebsite.Identity;

public static class RegisterIdentity
{

    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DbConnection")));

        services.AddTransient<IAccountServices, AccountServices>();


        services.AddIdentityCookie(configuration);
        services.AddJWTBearer(configuration);


        return services;
    }

    private static void AddIdentityCookie(this IServiceCollection services, IConfiguration configuration)
    {
        var identitySettings = configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>();

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = false;
            options.Password.RequireDigit = identitySettings.PasswordRequireDigit;
            options.Password.RequiredLength = identitySettings.PasswordRequiredLength;
            options.Password.RequireNonAlphanumeric = identitySettings.PasswordRequireNonAlphanumeric;
            options.Password.RequireUppercase = identitySettings.PasswordRequireUppercase;
            options.Password.RequireLowercase = identitySettings.PasswordRequireLowercase;
            options.Tokens.EmailConfirmationTokenProvider = "emailConfirmation";
        })
        .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
        .AddDefaultTokenProviders()
        .AddTokenProvider<EmailConfirmationTokenProvider<AppUser>>("emailConfirmation");

        services.Configure<DataProtectionTokenProviderOptions>(opt =>
        opt.TokenLifespan = TimeSpan.FromHours(2));
        services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromDays(3));
    }

    private static void AddJWTBearer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));
        var jwtSettings = configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)
                    ),
                };

                options.Events = new JwtBearerEvents()
                {
                    // OnMessageReceived = context =>
                    // {
                    //     context.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                    //     if (!string.IsNullOrEmpty(accessToken))
                    //     {
                    //         context.Token = accessToken;
                    //     }
                    //     return Task.CompletedTask;
                    // },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new BaseResult(new Error(ErrorCode.AccessDenied, "You are not Authorized")));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new BaseResult(new Error(ErrorCode.AccessDenied, "You are not authorized to access this resource")));
                        return context.Response.WriteAsync(result);
                    },
                    OnTokenValidated = async context =>
                    {
                        var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<AppUser>>();
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        var securityStamp = claimsIdentity.FindFirst("AspNet.Identity.SecurityStamp");
                        if (securityStamp is null)
                            context.Fail("This token has no secuirty stamp");

                        var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                        if (validatedUser == null)
                            context.Fail("Token secuirty stamp is not valid.");
                    },

                };
            });

    }

}