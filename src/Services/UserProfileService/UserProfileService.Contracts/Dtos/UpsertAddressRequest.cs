namespace UserProfileService.Contracts.Dtos;

public sealed record UpsertAddressRequest(
    string FirstName,
    string LastName,
    string Country,
    string City,
    string Street,
    string House,
    string PostalCode);
