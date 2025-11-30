using CinemaTicketingSystem.Application.Catalog.Cinema;
using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;
using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema.Hall;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CinemaTicketingSystem.Application.Test;

public class CinemaAppServiceTests : BaseIntegrationTest
{
    private readonly ICinemaAppService _cinemaAppService;

    public CinemaAppServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _cinemaAppService = GetService<ICinemaAppService>();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateCinema()
    {
        // Arrange
        var request = new CreateCinemaRequest(
            "Test Cinema Complex",
            new AddressDto(
                "Turkey",
                "Istanbul",
                "Kad?köy",
                "Test Street 123",
                "34710",
                "Near metro station"));

        // Act
        var result = await _cinemaAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        
        var cinema = await DbContext.Cinemas
            .FirstOrDefaultAsync(c => c.Name == "Test Cinema Complex");
        Assert.NotNull(cinema);
        Assert.Equal("Istanbul", cinema.Address.City);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ShouldReturnError()
    {
        // Arrange
        var existingCinema = new Domain.BoundedContexts.Catalog.Cinema(
            "Existing Cinema",
            new Address("Turkey", "Istanbul", "Test", "Test St", "34000", "Test"));
        
        await DbContext.Cinemas.AddAsync(existingCinema);
        await DbContext.SaveChangesAsync();

        var request = new CreateCinemaRequest(
            "Existing Cinema",
            new AddressDto("Turkey", "Ankara", "Test", "Test", "06000", "Test"));

        // Act
        var result = await _cinemaAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsFail);
        Assert.Contains("CinemaAlreadyExists", result.ProblemDetails?.Title ?? "");
    }

    [Fact]
    public async Task AddHallAsync_WithValidData_ShouldAddHallToCinema()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.FirstAsync();
        
        var request = new AddCinemaHallRequest(
            "New Hall Premium",
            new int[] { (int)ScreeningTechnology.IMAX, (int)ScreeningTechnology.ThreeD },
            new List<SeatDto>
            {
                new("A", 1, SeatType.Regular),
                new("A", 2, SeatType.VIP),
                new("B", 1, SeatType.Accessible)
            });

        // Act
        var result = await _cinemaAppService.AddHallAsync(cinema.Id, request);

        // Assert
        Assert.True(result.IsSuccess);
        
        var updatedCinema = await DbContext.Cinemas
            .Include(c => c.Halls)
            .ThenInclude(h => h.Seats)
            .FirstAsync(c => c.Id == cinema.Id);
        
        var newHall = updatedCinema.Halls.FirstOrDefault(h => h.Name == "New Hall Premium");
        Assert.NotNull(newHall);
        Assert.Equal(3, newHall.Seats.Count);
        Assert.True(newHall.SupportsTechnology(ScreeningTechnology.IMAX));
        Assert.True(newHall.SupportsTechnology(ScreeningTechnology.ThreeD));
    }

    [Fact]
    public async Task AddHallAsync_WithDuplicateHallName_ShouldReturnError()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.Include(c => c.Halls).FirstAsync();
        var existingHallName = cinema.Halls.First().Name;

        var request = new AddCinemaHallRequest(
            existingHallName,
            new int[] { (int)ScreeningTechnology.Standard },
            new List<SeatDto> { new("A", 1, SeatType.Regular) });

        // Act
        var result = await _cinemaAppService.AddHallAsync(cinema.Id, request);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task AddHallAsync_WithNonExistentCinema_ShouldReturnError()
    {
        // Arrange
        var nonExistentCinemaId = Guid.NewGuid();
        
        var request = new AddCinemaHallRequest(
            "Test Hall",
            new int[] { (int)ScreeningTechnology.Standard },
            new List<SeatDto> { new("A", 1, SeatType.Regular) });

        // Act
        var result = await _cinemaAppService.AddHallAsync(nonExistentCinemaId, request);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task RemoveHallAsync_WithExistingHall_ShouldRemoveHall()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.Include(c => c.Halls).FirstAsync();
        var hallToRemove = cinema.Halls.First();
        var initialHallCount = cinema.Halls.Count;

        var request = new RemoveCinemaHallRequest(cinema.Id, hallToRemove.Id);

        // Act
        var result = await _cinemaAppService.RemoveHallAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        
        var updatedCinema = await DbContext.Cinemas
            .Include(c => c.Halls)
            .FirstAsync(c => c.Id == cinema.Id);
        
        Assert.Equal(initialHallCount - 1, updatedCinema.Halls.Count);
        Assert.DoesNotContain(updatedCinema.Halls, h => h.Id == hallToRemove.Id);
    }

    [Fact]
    public async Task RemoveHallAsync_WithNonExistentHall_ShouldReturnError()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.FirstAsync();
        
        var request = new RemoveCinemaHallRequest(cinema.Id, Guid.NewGuid());

        // Act
        var result = await _cinemaAppService.RemoveHallAsync(request);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task GetCinemaHallsAsync_WithExistingCinema_ShouldReturnHalls()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.Include(c => c.Halls).FirstAsync();

        // Act
        var result = await _cinemaAppService.GetCinemaHallsAsync(cinema.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
    }

    [Fact]
    public async Task GetCinemaHallsAsync_WithNonExistentCinema_ShouldReturnError()
    {
        // Arrange
        var nonExistentCinemaId = Guid.NewGuid();

        // Act
        var result = await _cinemaAppService.GetCinemaHallsAsync(nonExistentCinemaId);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task GetAsync_WithExistingCinema_ShouldReturnCinema()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.FirstAsync();

        // Act
        var result = await _cinemaAppService.GetAsync(cinema.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(cinema.Id, result.Data.Id);
        Assert.Equal(cinema.Name, result.Data.Name);
    }

    [Fact]
    public async Task GetAsync_WithNonExistentCinema_ShouldReturnError()
    {
        // Arrange
        var nonExistentCinemaId = Guid.NewGuid();

        // Act
        var result = await _cinemaAppService.GetAsync(nonExistentCinemaId);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCinemas()
    {
        // Arrange
        var initialCount = (await DbContext.Cinemas.ToListAsync()).Count;

        // Act
        var result = await _cinemaAppService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(initialCount, result.Data.Count);
    }

    [Fact]
    public async Task GetAllAsync_WithNoCinemas_ShouldReturnEmptyList()
    {
        // Arrange
        var allCinemas = await DbContext.Cinemas.ToListAsync();
        DbContext.Cinemas.RemoveRange(allCinemas);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _cinemaAppService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task AddHallAsync_WithMultipleTechnologies_ShouldSupportAllTechnologies()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.FirstAsync();
        
        var request = new AddCinemaHallRequest(
            "Multi-Tech Hall",
            new int[] 
            { 
                (int)ScreeningTechnology.Standard,
                (int)ScreeningTechnology.IMAX,
                (int)ScreeningTechnology.ThreeD,
                (int)ScreeningTechnology.FourDX
            },
            new List<SeatDto> { new("A", 1, SeatType.Regular) });

        // Act
        var result = await _cinemaAppService.AddHallAsync(cinema.Id, request);

        // Assert
        Assert.True(result.IsSuccess);
        
        var updatedCinema = await DbContext.Cinemas
            .Include(c => c.Halls)
            .FirstAsync(c => c.Id == cinema.Id);
        
        var hall = updatedCinema.Halls.First(h => h.Name == "Multi-Tech Hall");
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.Standard));
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.IMAX));
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.ThreeD));
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.FourDX));
    }
}
