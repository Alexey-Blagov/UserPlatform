using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserProfileService.Domain.Repositories;
using UserProfileService.Infrastructure.Persistence;

namespace UserProfileService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUserProfileInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserProfileDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.AddScoped<IUserProfileRepository, UserProfileRepository>();

        return services;
    }
}
