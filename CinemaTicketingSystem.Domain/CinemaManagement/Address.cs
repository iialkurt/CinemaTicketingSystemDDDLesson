namespace CinemaTicketingSystem.Domain.CinemaManagement;

public class Address : ValueObject
{
    public Address(
        string country,
        string city,
        string district,
        string street,
        string postalCode,
        string? description = null)
    {
        Country = country;
        City = city;
        District = district;
        Street = street;
        PostalCode = postalCode;
        Description = description;
    }

    public string Country { get; }
    public string City { get; }
    public string District { get; }
    public string Street { get; }
    public string PostalCode { get; }
    public string Description { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Country;
        yield return City;
        yield return District;
        yield return Street;
        yield return PostalCode;
        yield return Description;
    }
}