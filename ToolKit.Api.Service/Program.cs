using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToolKit.Api.Business.Extensions;
using ToolKit.Api.Business.Factories;
using ToolKit.Api.Business.Managers;
using ToolKit.Api.Business.Managers.GitHub;
using ToolKit.Api.Business.Providers.GitHub;
using ToolKit.Api.Data.Repositories;
using ToolKit.Api.DataModel;
using ToolKit.Api.Interfaces.Factories;
using ToolKit.Api.Interfaces.Managers;
using ToolKit.Api.Interfaces.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;
using ToolKit.Api.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services.AddDbContext<ToolKitContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddAuthorization(options =>
    {
        // Example of setting up a policy to require a scope
        options.AddPolicy(
            "read:users",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("read:users", domain)
            )
        );
    });

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddScoped<IUsersManager, UsersManager>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IGitHubAuthManager, GitHubAuthManager>();
builder.Services.AddScoped<IGitHubAuthProvider, GitHubAuthProvider>();

var privateKey = configuration["GitHub:PrivateKey"];
var appId = int.Parse(configuration["GitHub:AppId"]);

builder.Services.AddSingleton<IGitHubJwtTokenManager>(new GitHubJwtTokenManager(privateKey, appId));
builder.Services.AddScoped<IGitHubApplicationManager, GitHubApplicationManager>();
builder.Services.AddScoped<IGitHubApplicationProvider, GitHubApplicationProvider>();
builder.Services.AddScoped<IGitHubClientFactory, GitHubClientFactory>();
builder.Services.AddScoped<IGitHubInstallationsManager, GitHubInstallationsManager>();
builder.Services.AddScoped<IGitHubInstallationsProvider, GitHubInstallationsProvider>();
builder.Services.AddScoped<IGitHubUserReposManager, GitHubUserReposManager>();
builder.Services.AddScoped<IGitHubUserReposProvider, GitHubUserReposProvider>();
builder.Services.AddScoped<IGitHubIssuesManager, GitHubIssuesManager>();
builder.Services.AddScoped<IGitHubIssuesProvider, GitHubIssuesProvider>();

builder.Services.AddHttpClient("GitHub", client =>
{
    client.BaseAddress = new Uri("https://github.com");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
});
builder.Services.AddHttpClient("GitHubApi", client =>
{
    client.BaseAddress = new Uri("https://api.github.com");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => $"{type.FullName.Replace('.', '_')}");

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"https://{builder.Configuration["Auth0:Domain"]}/authorize"),
                TokenUrl = new Uri($"https://{builder.Configuration["Auth0:Domain"]}/oauth/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "read:users", "Read users" }
                }
            }
        }
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "read:users" }
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";

        // Configure Swagger UI to use Auth0
        c.OAuthClientId(builder.Configuration["Auth0:ClientId"]);
        c.OAuthClientSecret(builder.Configuration["Auth0:ClientSecret"]); // Only if you are using a confidential client
        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
        {
            { "audience", builder.Configuration["Auth0:Audience"] }
        });
        c.OAuthAppName("Swagger UI");
        c.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();