using Microsoft.AspNetCore.Identity;
using MyOwnWebsite.Application.Contracts;
using MyOwnWebsite.Domain.Settings;
using MyOwnWebsite.Api.Endpoints;
using MyOwnWebsite.Identity;
using MyOwnWebsite.Identity.Models;
using MyOwnWebsite.Identity.Seeds;
using MyOwnWebsite.Api.Infrastructure.Services;
using MyOwnWebsite.Persistence;
using MyOwnWebsite.Application;
using MyOwnWebsite.Api.Infrastructure.Middlewares;
using MyOwnWebsite.Domain;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration
.AddEnvironmentVariables("SUL_");

IConfiguration configuration = builder.Configuration;


var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
builder.Services.AddSingleton(corsSettings);
builder.Services.AddCors(options =>
{


    options.AddPolicy("AppCors", builder =>
    {
        builder
        .SetIsOriginAllowed(origin =>
        {
            if (string.IsNullOrWhiteSpace(origin)) return false;
            if (string.Equals(origin, corsSettings.AllowedOrigin)) return true;
            return false;
        })

                .AllowAnyMethod()
                .AllowAnyHeader()
            .AllowCredentials();
    });
});


builder.Services.AddAuthorization();


builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.Configure<CorsSettings>(configuration.GetSection(nameof(CorsSettings)));
builder.Services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Host.UseSerilog((context, conf) => conf.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApplication()
    .AddPersistence(configuration)
    .AddIdentity(configuration);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAntiforgery();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
//app.UseAntiforgery();
app.UseStaticFiles();
app.UseCors("AppCors");



app.UseAuthentication();
app.UseAuthorization();

app.UseGlobalErrorHandlerMiddleware();

app.MapProfileEndpoint();
app.AddProjectsEndpoint();
app.AddAccountEndpoint();
app.AddStatisticsEndpoint();




using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DefaultRoles.SeedAsync(services.GetRequiredService<RoleManager<AppRole>>());
    await DefaultAdminUserData.SeedAsync(services.GetRequiredService<UserManager<AppUser>>());
}



app.Run();




