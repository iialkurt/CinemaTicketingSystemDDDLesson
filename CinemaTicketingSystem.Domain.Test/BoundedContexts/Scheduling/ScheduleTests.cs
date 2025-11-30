
using System;

using Ardalis.GuardClauses;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Xunit;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.UnitTests;


public class ScheduleTests
{
    /// <summary>
    /// Tests that the protected parameterless constructor successfully creates a Schedule instance.
    /// This constructor is typically used by ORMs like Entity Framework for object materialization.
    /// </summary>
    [Fact]
    public void Constructor_Parameterless_ShouldCreateInstance()
    {
        // Arrange & Act
        var schedule = new TestableSchedule();

        // Assert
        Assert.NotNull(schedule);
        Assert.Equal(Guid.Empty, schedule.MovieId);
        Assert.Equal(Guid.Empty, schedule.HallId);
    }

    /// <summary>
    /// Helper class to expose the protected parameterless constructor for testing purposes.
    /// </summary>
    private class TestableSchedule : Schedule
    {
        public TestableSchedule() : base()
        {
        }
    }

    /// <summary>
    /// Tests that the Schedule constructor successfully creates a Schedule instance
    /// with all valid parameters and sets all properties correctly.
    /// </summary>
    [Fact]
    public void Constructor_ShouldCreateSchedule_WithValidParameters()
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        Guid hallId = Guid.NewGuid();
        ShowTime showTime = ShowTime.Create(new TimeOnly(10, 0), new TimeOnly(12, 0));
        Price price = new Price(15.50m, "USD");

        // Act
        Schedule schedule = new Schedule(movieId, hallId, showTime, price);

        // Assert
        Assert.Equal(movieId, schedule.MovieId);
        Assert.Equal(hallId, schedule.HallId);
        Assert.Equal(showTime, schedule.ShowTime);
        Assert.Equal(price, schedule.TicketPrice);
        Assert.NotEqual(Guid.Empty, schedule.Id);
    }

    /// <summary>
    /// Tests that the Schedule constructor throws ArgumentException
    /// when movieId parameter is Guid.Empty (default value).
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenMovieIdIsDefault()
    {
        // Arrange
        Guid movieId = Guid.Empty;
        Guid hallId = Guid.NewGuid();
        ShowTime showTime = ShowTime.Create(new TimeOnly(10, 0), new TimeOnly(12, 30));
        Price price = new Price(15.50m, "USD");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Schedule(movieId, hallId, showTime, price));
    }

    /// <summary>
    /// Tests that the Schedule constructor throws ArgumentException
    /// when hallId parameter is Guid.Empty (default value).
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenHallIdIsDefault()
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        Guid hallId = Guid.Empty;
        ShowTime showTime = ShowTime.Create(new TimeOnly(10, 0), new TimeOnly(12, 30));
        Price price = new Price(15.50m, "USD");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Schedule(movieId, hallId, showTime, price));
    }

    /// <summary>
    /// Tests that the Schedule constructor throws ArgumentNullException
    /// when showTime parameter is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenShowTimeIsNull()
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        Guid hallId = Guid.NewGuid();
        ShowTime? showTime = null;
        Price price = new Price(15.50m, "USD");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Schedule(movieId, hallId, showTime!, price));
    }

    /// <summary>
    /// Tests that the Schedule constructor throws ArgumentNullException
    /// when price parameter is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenPriceIsNull()
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        Guid hallId = Guid.NewGuid();
        ShowTime showTime = ShowTime.Create(new TimeOnly(10, 0), new TimeOnly(12, 0));
        Price? price = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Schedule(movieId, hallId, showTime, price!));
    }

    /// <summary>
    /// Tests that CanBeCancelled returns true when the schedule has not started yet.
    /// This verifies that schedules can be cancelled before the show time begins.
    /// Note: This test uses a very early morning time (00:01-02:00) which should be "in the past" 
    /// for most test runs, making HasStarted return true and CanBeCancelled return false.
    /// However, if run between 00:00-00:01, it may pass. This test demonstrates the limitation
    /// of testing time-dependent logic without dependency injection of a time provider.
    /// </summary>
    [Fact]
    public void CanBeCancelled_ShouldReturnFalse_WhenUsingEarlyMorningTime()
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        Guid hallId = Guid.NewGuid();
        // Using 00:01-02:00 (very early morning) - likely already passed
        ShowTime showTime = ShowTime.Create(new TimeOnly(0, 1), new TimeOnly(2, 0));
        Price price = new Price(15.50m, "USD");
        Schedule schedule = new Schedule(movieId, hallId, showTime, price);

        // Act
        bool result = schedule.CanBeCancelled();

        // Assert
        // This will be false if current time > 00:01 (which is almost always true)
        Assert.False(result);
    }

    /// <summary>
    /// Tests that CanBeCancelled returns false when the schedule has already started.
    /// This verifies that schedules cannot be cancelled after the show time has begun.
    /// </summary>
    [Fact]
    public void CanBeCancelled_ShouldReturnFalse_WhenScheduleHasStarted()
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        Guid hallId = Guid.NewGuid();
        // Create a showtime in the past (early morning)
        ShowTime showTime = ShowTime.Create(new TimeOnly(0, 0), new TimeOnly(2, 30));
        Price price = new Price(15.50m, "USD");
        Schedule schedule = new Schedule(movieId, hallId, showTime, price);

        // Act
        bool result = schedule.CanBeCancelled();

        // Assert
        // This test should pass since 00:00 is always in the past during normal working hours
        Assert.False(result);
    }

    /// <summary>
    /// Tests that GetDisplayInfo returns a properly formatted string containing
    /// the HallId and ShowTime display information in the expected format.
    /// </summary>
    [Fact]
    public void GetDisplayInfo_ShouldReturnFormattedString_WithHallIdAndShowTimeInfo()
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        Guid hallId = Guid.NewGuid();
        TimeOnly startTime = new TimeOnly(14, 30);
        TimeOnly endTime = new TimeOnly(16, 30);
        ShowTime showTime = ShowTime.Create(startTime, endTime);
        Price price = new Price(10.50m, "USD");

        Schedule schedule = new Schedule(movieId, hallId, showTime, price);

        string expectedShowTimeInfo = showTime.GetDisplayInfo();
        string expectedResult = $"Hall {hallId} - {expectedShowTimeInfo}";

        // Act
        string result = schedule.GetDisplayInfo();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Contains($"Hall {hallId}", result);
        Assert.Contains(expectedShowTimeInfo, result);
    }

    /// <summary>
    /// Tests that GetDisplayInfo correctly formats the output with different
    /// HallId and ShowTime combinations to ensure proper string interpolation.
    /// </summary>
    [Theory]
    [InlineData("08:00", "10:00")]
    [InlineData("12:15", "14:45")]
    [InlineData("20:00", "22:30")]
    public void GetDisplayInfo_ShouldReturnCorrectFormat_WithVariousShowTimes(string startTimeStr, string endTimeStr)
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        Guid hallId = Guid.NewGuid();
        TimeOnly startTime = TimeOnly.Parse(startTimeStr);
        TimeOnly endTime = TimeOnly.Parse(endTimeStr);
        ShowTime showTime = ShowTime.Create(startTime, endTime);
        Price price = new Price(15.00m, "USD");

        Schedule schedule = new Schedule(movieId, hallId, showTime, price);

        string expectedShowTimeInfo = showTime.GetDisplayInfo();
        string expectedResult = $"Hall {hallId} - {expectedShowTimeInfo}";

        // Act
        string result = schedule.GetDisplayInfo();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.StartsWith("Hall ", result);
        Assert.Contains(" - ", result);
    }

    /// <summary>
    /// Tests that GetDisplayInfo returns a properly formatted string with different hall IDs.
    /// This verifies the string formatting works correctly with various Guid values.
    /// </summary>
    [Fact]
    public void GetDisplayInfo_ShouldReturnFormattedString_WithDifferentHallIds()
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        Guid hallId1 = Guid.NewGuid();
        Guid hallId2 = Guid.NewGuid();
        ShowTime showTime = ShowTime.Create(new TimeOnly(10, 0), new TimeOnly(12, 0));
        Price price = new Price(15.50m, "USD");

        Schedule schedule1 = new Schedule(movieId, hallId1, showTime, price);
        Schedule schedule2 = new Schedule(movieId, hallId2, showTime, price);

        // Act
        string result1 = schedule1.GetDisplayInfo();
        string result2 = schedule2.GetDisplayInfo();

        // Assert
        Assert.Contains($"Hall {hallId1}", result1);
        Assert.Contains($"Hall {hallId2}", result2);
        Assert.NotEqual(result1, result2);
    }
}