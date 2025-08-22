namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

public enum HoldStatus
{
    Active,
    Confirm,
    Expired,
    ConvertedToReservation,
    ConvertedToPurchase
}