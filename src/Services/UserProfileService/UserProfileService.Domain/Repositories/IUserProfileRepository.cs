using UserProfileService.Domain.Entities;

namespace UserProfileService.Domain.Repositories;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByExternalIdentityIdAsync(string externalIdentityId, CancellationToken cancellationToken);
    Task AddAsync(UserProfile profile, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
