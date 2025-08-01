using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;

namespace CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema;

public record CreateCinemaRequest(string Name, AddressDto Address);