using System.Security.Claims;
using System.Text.Json;
using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Behaviors;
using BuildingBlocks.Infrastructure.Security;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserProfileService.API.Keycloak;
using UserProfileService.Application.Commands.UpsertMyAddress;
using UserProfileService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection("Keycloak"));
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddUserProfileInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<UpsertMyAddressCommand>();
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblyContaining<UpsertMyAddressCommandValidator>();
builder.Services.AddScoped<ICurrentUser, HttpContextCurrentUser>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = bool.TryParse(builder.Configuration["Keycloak:RequireHttpsMetadata"], out var requireHttps) && requireHttps;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            NameClaimType = "preferred_username",
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                if (context.Principal?.Identity is not ClaimsIdentity identity)
                    return Task.CompletedTask;

                AddRolesFromRealmAccess(identity, context.Principal.FindFirst("realm_access")?.Value);

                var resourceAccess = context.Principal.FindFirst("resource_access")?.Value;
                if (!string.IsNullOrWhiteSpace(resourceAccess))
                {
                    using var doc = JsonDocument.Parse(resourceAccess);
                    foreach (var client in doc.RootElement.EnumerateObject())
                    {
                        if (client.Value.TryGetProperty("roles", out var rolesElement))
                        {
                            foreach (var role in rolesElement.EnumerateArray())
                            {
                                var roleValue = role.GetString();
                                if (!string.IsNullOrWhiteSpace(roleValue))
                                    identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                            }
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", p => p.RequireRole("user", "admin"));
    options.AddPolicy("AdminPolicy", p => p.RequireRole("admin"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddRolesFromRealmAccess(ClaimsIdentity identity, string? realmAccessJson)
{
    if (string.IsNullOrWhiteSpace(realmAccessJson))
        return;

    using var doc = JsonDocument.Parse(realmAccessJson);
    if (!doc.RootElement.TryGetProperty("roles", out var rolesElement))
        return;

    foreach (var role in rolesElement.EnumerateArray())
    {
        var roleValue = role.GetString();
        if (!string.IsNullOrWhiteSpace(roleValue))
            identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
    }
}

public partial class Program;
