using BuildingBlocks.Domain;
using UserProfileService.Domain.ValueObjects;

namespace UserProfileService.Domain.Entities;

public sealed class UserProfile : Entity<Guid>
{
    private UserProfile()
    {
        ExternalIdentityId = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Address = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
    }

    public UserProfile(Guid id, string externalIdentityId, string firstName, string lastName, Address address)
    {
        Id = id;
        ExternalIdentityId = externalIdentityId;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
    }

    public string ExternalIdentityId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Address Address { get; private set; }

    public void UpdateAddress(Address address) => Address = address;
}
