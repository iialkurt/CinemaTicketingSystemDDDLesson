
using System;

using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.SharedKernel;
using Xunit;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.UnitTests;


/// <summary>
/// Unit tests for the <see cref="CinemaHallSnapshot"/> class.
/// </summary>
public class CinemaHallSnapshotTests
{
    /// <summary>
    /// Tests that the constructor successfully creates a CinemaHallSnapshot instance
    /// with valid parameters and correctly sets all properties.
    /// </summary>
    /// <param name="seatCount">The seat count to test.</param>
    /// <param name="technology">The screening technology to test.</param>
    [Theory]
    [InlineData((short)1, ScreeningTechnology.Standard)]
    [InlineData((short)100, ScreeningTechnology.IMAX)]
    [InlineData((short)500, ScreeningTechnology.ThreeD)]
    [InlineData((short)32767, ScreeningTechnology.FourDX)]
    [InlineData((short)250, ScreeningTechnology.Standard | ScreeningTechnology.IMAX)]
    [InlineData((short)50, ScreeningTechnology.IMAX | ScreeningTechnology.ThreeD)]
    public void Constructor_ShouldCreateInstance_WithValidParameters(short seatCount, ScreeningTechnology technology)
    {
        // Arrange
        Guid cinemaHallId = Guid.NewGuid();

        // Act
        CinemaHallSnapshot snapshot = new CinemaHallSnapshot(cinemaHallId, seatCount, technology);

        // Assert
        Assert.Equal(cinemaHallId, snapshot.Id);
        Assert.Equal(seatCount, snapshot.SeatCount);
        Assert.Equal(technology, snapshot.SupportedTechnologies);
    }

    /// <summary>
    /// Tests that the constructor throws ArgumentException when cinemaHallId is Guid.Empty.
    /// Verifies the exception type, message, and parameter name.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenCinemaHallIdIsEmpty()
    {
        // Arrange
        Guid emptyCinemaHallId = Guid.Empty;
        short seatCount = 100;
        ScreeningTechnology technology = ScreeningTechnology.Standard;

        // Act & Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            new CinemaHallSnapshot(emptyCinemaHallId, seatCount, technology));

        Assert.Equal("cinemaHallId", exception.ParamName);
        Assert.Contains("Cinema hall ID cannot be empty", exception.Message);
    }

    /// <summary>
    /// Tests that the constructor throws ArgumentOutOfRangeException when seatCount is zero or negative.
    /// Verifies the exception type, message, and parameter name for various invalid seat count values.
    /// </summary>
    /// <param name="invalidSeatCount">The invalid seat count value to test.</param>
    [Theory]
    [InlineData((short)0)]
    [InlineData((short)-1)]
    [InlineData((short)-100)]
    [InlineData(short.MinValue)]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenSeatCountIsZeroOrNegative(short invalidSeatCount)
    {
        // Arrange
        Guid cinemaHallId = Guid.NewGuid();
        ScreeningTechnology technology = ScreeningTechnology.Standard;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CinemaHallSnapshot(cinemaHallId, invalidSeatCount, technology));

        Assert.Equal("seatCount", exception.ParamName);
        Assert.Contains("Seat count must be positive", exception.Message);
    }

    /// <summary>
    /// Tests that the constructor accepts invalid enum values for ScreeningTechnology
    /// since there is no validation for this parameter.
    /// </summary>
    /// <param name="invalidTechnology">The invalid enum value to test.</param>
    [Theory]
    [InlineData((ScreeningTechnology)0)]
    [InlineData((ScreeningTechnology)999)]
    [InlineData((ScreeningTechnology)(-1))]
    public void Constructor_ShouldAcceptInvalidEnumValue_WhenScreeningTechnologyIsOutOfRange(ScreeningTechnology invalidTechnology)
    {
        // Arrange
        Guid cinemaHallId = Guid.NewGuid();
        short seatCount = 100;

        // Act
        CinemaHallSnapshot snapshot = new CinemaHallSnapshot(cinemaHallId, seatCount, invalidTechnology);

        // Assert
        Assert.Equal(cinemaHallId, snapshot.Id);
        Assert.Equal(seatCount, snapshot.SeatCount);
        Assert.Equal(invalidTechnology, snapshot.SupportedTechnologies);
    }

    /// <summary>
    /// Tests that the protected parameterless constructor creates a valid instance
    /// with default property values.
    /// </summary>
    [Fact]
    public void Constructor_Parameterless_ShouldCreateInstance()
    {
        // Arrange & Act
        CinemaHallSnapshotTestHelper instance = new CinemaHallSnapshotTestHelper();

        // Assert
        Assert.NotNull(instance);
        Assert.Equal(ScreeningTechnology.Standard, instance.SupportedTechnologies);
        Assert.Equal(0, instance.SeatCount);
    }

    /// <summary>
    /// Helper class to expose the protected parameterless constructor for testing purposes.
    /// </summary>
    private class CinemaHallSnapshotTestHelper : CinemaHallSnapshot
    {
        public CinemaHallSnapshotTestHelper() : base()
        {
        }
    }
}