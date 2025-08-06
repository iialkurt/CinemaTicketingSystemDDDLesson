#region

using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;

#endregion

namespace CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema;

public record CreateCinemaRequest(string Name, AddressDto Address);