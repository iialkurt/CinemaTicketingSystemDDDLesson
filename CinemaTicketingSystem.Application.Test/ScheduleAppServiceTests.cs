using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.Schedule;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Application.Schedules;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CinemaTicketingSystem.Application.Test;

public class ScheduleAppServiceTests : BaseIntegrationTest
{
    private readonly IScheduleAppService _scheduleAppService;

    public ScheduleAppServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _scheduleAppService = GetService<IScheduleAppService>();
    }

    [Fact]
    public async Task AddMovieToHall_WithValidData_ShouldCreateSchedule()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.Include(c => c.Halls).FirstAsync();
        var hall = cinema.Halls.First();
        var movie = await DbContext.Movies.FirstAsync();

        var request = new AddMovieToHallRequest(
            movie.Id,
            new TimeOnly(14, 0),
            new TimeOnly(16, 30),
            new PriceDto(50, "TRY"));

        // Act
        var result = await _scheduleAppService.AddMovieToHall(hall.Id, request);

        // Assert
        Assert.True(result.IsSuccess);
        var schedule = await DbContext.Schedules.FirstOrDefaultAsync(s => 
            s.MovieId == movie.Id && s.HallId == hall.Id);
        Assert.NotNull(schedule);
        Assert.Equal(new TimeOnly(14, 0), schedule.ShowTime.StartTime);
    }

    [Fact]
    public async Task AddMovieToHall_WithConflictingShowTime_ShouldReturnError()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.Include(c => c.Halls).FirstAsync();
        var hall = cinema.Halls.First();
        var movie = await DbContext.Movies.FirstAsync();

        // Create existing schedule
        var existingShowTime = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var existingSchedule = new Schedule(movie.Id, hall.Id, existingShowTime, new Price(50, "TRY"));
        await DbContext.Schedules.AddAsync(existingSchedule);
        await DbContext.SaveChangesAsync();

        var request = new AddMovieToHallRequest(
            movie.Id,
            new TimeOnly(15, 0), // Conflicts with existing
            new TimeOnly(17, 0),
            new PriceDto(50, "TRY"));

        // Act
        var result = await _scheduleAppService.AddMovieToHall(hall.Id, request);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task AddMovieToHall_WithNonExistentMovie_ShouldReturnError()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.Include(c => c.Halls).FirstAsync();
        var hall = cinema.Halls.First();
        var nonExistentMovieId = Guid.NewGuid();

        var request = new AddMovieToHallRequest(
            nonExistentMovieId,
            new TimeOnly(14, 0),
            new TimeOnly(16, 0),
            new PriceDto(50, "TRY"));

        // Act
        var result = await _scheduleAppService.AddMovieToHall(hall.Id, request);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task AddMovieToHall_WithNonExistentHall_ShouldReturnError()
    {
        // Arrange
        var movie = await DbContext.Movies.FirstAsync();
        var nonExistentHallId = Guid.NewGuid();

        var request = new AddMovieToHallRequest(
            movie.Id,
            new TimeOnly(14, 0),
            new TimeOnly(16, 0),
            new PriceDto(50, "TRY"));

        // Act
        var result = await _scheduleAppService.AddMovieToHall(nonExistentHallId, request);

        // Assert
        Assert.True(result.IsFail);
    }

    [Fact]
    public async Task AddMovieToHall_WithDurationFromMovie_ShouldCalculateEndTime()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.Include(c => c.Halls).FirstAsync();
        var hall = cinema.Halls.First();
        var movie = await DbContext.Movies.FirstAsync();

        var request = new AddMovieToHallRequest(
            movie.Id,
            new TimeOnly(14, 0),
            null, // Will use movie duration
            new PriceDto(50, "TRY"));

        // Act
        var result = await _scheduleAppService.AddMovieToHall(hall.Id, request);

        // Assert
        Assert.True(result.IsSuccess);
        var schedule = await DbContext.Schedules.FirstOrDefaultAsync(s => 
            s.MovieId == movie.Id && s.HallId == hall.Id);
        Assert.NotNull(schedule);
    }

    [Fact]
    public async Task GetMoviesByHallId_WithExistingSchedules_ShouldReturnSchedules()
    {
        // Arrange
        var cinema = await DbContext.Cinemas.Include(c => c.Halls).FirstAsync();
        var hall = cinema.Halls.First();
        var movie = await DbContext.Movies.FirstAsync();

        var showTime1 = ShowTime.Create(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        
        var schedule1 = new Schedule(movie.Id, hall.Id, showTime1, new Price(40, "TRY"));
        var schedule2 = new Schedule(movie.Id, hall.Id, showTime2, new Price(50, "TRY"));
        
        await DbContext.Schedules.AddAsync(schedule1);
        await DbContext.Schedules.AddAsync(schedule2);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _scheduleAppService.GetMoviesByHallId(hall.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Count >= 2);
    }

    [Fact]
    public async Task GetMoviesByHallId_WithNoSchedules_ShouldReturnEmptyList()
    {
        // Arrange
        var cinema = new Cinema("Test Cinema", 
            new Address("Turkey", "Istanbul", "Test", "Test St", "34000", "Test"));
        var hall = new CinemaHall("Test Hall", ScreeningTechnology.Standard);
        cinema.AddHall(hall);
        
        await DbContext.Cinemas.AddAsync(cinema);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _scheduleAppService.GetMoviesByHallId(hall.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }
}
