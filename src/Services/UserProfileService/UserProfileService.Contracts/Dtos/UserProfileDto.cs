namespace UserProfileService.Contracts.Dtos;

public sealed record UserProfileDto(
    Guid Id,
    string ExternalIdentityId,
    string FirstName,
    string LastName,
    string Country,
    string City,
    string Street,
    string House,
    string PostalCode);
