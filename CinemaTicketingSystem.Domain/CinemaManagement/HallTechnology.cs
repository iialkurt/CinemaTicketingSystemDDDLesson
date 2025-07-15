namespace CinemaTicketingSystem.Domain.CinemaManagement;

[Flags]
public enum HallTechnology
{
    None = 0,
    Standard = 1,
    IMAX = 2,
    ThreeD = 4,
    FourDX = 8,
    ScreenX = 16,
    DolbyAtmos = 32,
    DolbyCinema = 64
}