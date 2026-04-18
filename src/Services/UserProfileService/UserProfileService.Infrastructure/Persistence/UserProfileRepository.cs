using Microsoft.EntityFrameworkCore;
using UserProfileService.Domain.Entities;
using UserProfileService.Domain.Repositories;

namespace UserProfileService.Infrastructure.Persistence;

public sealed class UserProfileRepository : IUserProfileRepository
{
    private readonly UserProfileDbContext _dbContext;

    public UserProfileRepository(UserProfileDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UserProfile?> GetByExternalIdentityIdAsync(string externalIdentityId, CancellationToken cancellationToken)
        => _dbContext.UserProfiles.FirstOrDefaultAsync(x => x.ExternalIdentityId == externalIdentityId, cancellationToken);

    public Task AddAsync(UserProfile profile, CancellationToken cancellationToken)
        => _dbContext.UserProfiles.AddAsync(profile, cancellationToken).AsTask();

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
