namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema
{
    public record CreateCinemaRequest(string Name, AddressDto Address);



    public record AddressDto(
        string Country,
        string City,
        string District,
        string Street,
        string PostalCode,
        string? Description = null);
}
