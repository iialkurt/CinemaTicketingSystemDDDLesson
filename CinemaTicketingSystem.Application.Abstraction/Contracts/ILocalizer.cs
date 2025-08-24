namespace CinemaTicketingSystem.Application.Contracts.Contracts;

public interface ILocalizer
{
    string L(string key, object[]? data = null);
}