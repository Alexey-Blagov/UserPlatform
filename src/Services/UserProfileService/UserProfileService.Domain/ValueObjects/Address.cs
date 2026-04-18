using BuildingBlocks.Domain;

namespace UserProfileService.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string House { get; }
    public string PostalCode { get; }

    private Address()
    {
        Country = string.Empty;
        City = string.Empty;
        Street = string.Empty;
        House = string.Empty;
        PostalCode = string.Empty;
    }

    public Address(string country, string city, string street, string house, string postalCode)
    {
        Country = country;
        City = city;
        Street = street;
        House = house;
        PostalCode = postalCode;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Country;
        yield return City;
        yield return Street;
        yield return House;
        yield return PostalCode;
    }
}
