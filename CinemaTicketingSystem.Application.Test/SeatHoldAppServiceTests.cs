using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Application.Ticketing;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CinemaTicketingSystem.Application.Test;

public class SeatHoldAppServiceTests : BaseIntegrationTest
{
    private readonly ISeatHoldAppService _seatHoldAppService;
    private readonly ISeatHoldRepository _seatHoldRepository;
    private readonly Guid _testUserId;

    public SeatHoldAppServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _testUserId = GetTestUserId();
        
        // Replace UserContext with fake one
        var scope = factory.Services.CreateScope();
        var fakeUserContext = new FakeUserContext(_testUserId);
        
        var appDependencyService = scope.ServiceProvider.GetRequiredService<AppDependencyService>();
        typeof(AppDependencyService)
            .GetProperty(nameof(AppDependencyService.UserContext))!
            .SetValue(appDependencyService, fakeUserContext);
        
        _seatHoldAppService = scope.ServiceProvider.GetRequiredService<ISeatHoldAppService>();
        _seatHoldRepository = scope.ServiceProvider.GetRequiredService<ISeatHoldRepository>();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldCreateSeatHolds()
    {
        // Arrange
        var scheduleId = Guid.NewGuid();
        var screeningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var request = new CreateSeatHoldRequest(
            new List<SeatPositionDto>
            {
                new("A", 1),
                new("A", 2)
            },
            scheduleId,
            screeningDate);

        // Act
        var result = await _seatHoldAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        var holds = await _seatHoldRepository.WhereAsync(x => 
            x.ScheduledMovieShowId == scheduleId && 
            x.CustomerId == _testUserId);
        Assert.Equal(2, holds.Count());
    }

    [Fact]
    public async Task CreateAsync_WithAlreadyHeldSeat_ShouldReturnError()
    {
        // Arrange
        var scheduleId = Guid.NewGuid();
        var screeningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var seatPosition = new SeatPosition("A", 1);
        
        // Create existing hold
        var existingHold = new SeatHold(scheduleId, Guid.NewGuid(), seatPosition, screeningDate);
        await _seatHoldRepository.AddAsync(existingHold);
        await DbContext.SaveChangesAsync();

        var request = new CreateSeatHoldRequest(
            new List<SeatPositionDto>
            {
                new("A", 1)
            },
            scheduleId,
            screeningDate);

        // Act
        var result = await _seatHoldAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsFail);
        Assert.Contains("SeatAlreadyHeld", result.ProblemDetails?.Title ?? "");
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateSeatsInRequest_ShouldBeIdempotent()
    {
        // Arrange
        var scheduleId = Guid.NewGuid();
        var screeningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var seatPosition = new SeatPosition("A", 1);
        
        // Create existing hold for same user
        var existingHold = new SeatHold(scheduleId, _testUserId, seatPosition, screeningDate);
        await _seatHoldRepository.AddAsync(existingHold);
        await DbContext.SaveChangesAsync();

        var request = new CreateSeatHoldRequest(
            new List<SeatPositionDto>
            {
                new("A", 1), // Already held by same user
                new("A", 2)  // New seat
            },
            scheduleId,
            screeningDate);

        // Act
        var result = await _seatHoldAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        var holds = await _seatHoldRepository.WhereAsync(x => 
            x.ScheduledMovieShowId == scheduleId && 
            x.CustomerId == _testUserId);
        Assert.Equal(2, holds.Count()); // Should have 2 total (1 existing + 1 new)
    }

    [Fact]
    public async Task ConfirmAsync_WithValidRequest_ShouldConfirmAllUserHolds()
    {
        // Arrange
        var scheduleId = Guid.NewGuid();
        var screeningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        
        var hold1 = new SeatHold(scheduleId, _testUserId, new SeatPosition("A", 1), screeningDate);
        var hold2 = new SeatHold(scheduleId, _testUserId, new SeatPosition("A", 2), screeningDate);
        
        await _seatHoldRepository.AddAsync(hold1);
        await _seatHoldRepository.AddAsync(hold2);
        await DbContext.SaveChangesAsync();

        var request = new ConfirmSeatHoldRequest(scheduleId, screeningDate);

        // Act
        var result = await _seatHoldAppService.ConfirmAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        var holds = await _seatHoldRepository.WhereAsync(x => 
            x.ScheduledMovieShowId == scheduleId && 
            x.CustomerId == _testUserId);
        Assert.All(holds, h => Assert.Equal(HoldStatus.Hold, h.Status));
    }

    [Fact]
    public async Task Cancel_ShouldDeleteAllUserHolds()
    {
        // Arrange
        var scheduleId1 = Guid.NewGuid();
        var scheduleId2 = Guid.NewGuid();
        var screeningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        
        var hold1 = new SeatHold(scheduleId1, _testUserId, new SeatPosition("A", 1), screeningDate);
        var hold2 = new SeatHold(scheduleId2, _testUserId, new SeatPosition("B", 1), screeningDate);
        
        await _seatHoldRepository.AddAsync(hold1);
        await _seatHoldRepository.AddAsync(hold2);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _seatHoldAppService.Cancel();

        // Assert
        Assert.True(result.IsSuccess);
        var holds = await _seatHoldRepository.WhereAsync(x => x.CustomerId == _testUserId);
        Assert.Empty(holds);
    }
}
