using System;
using System.Linq;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Xunit;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.UnitTests;

public class CinemaHallTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateCinemaHall()
    {
        // Arrange & Act
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);

        // Assert
        Assert.NotNull(hall);
        Assert.Equal("Hall 1", hall.Name);
        Assert.Equal(ScreeningTechnology.Standard, hall.SupportedTechnologies);
        Assert.True(hall.IsOperational);
        Assert.Empty(hall.Seats);
        Assert.NotEqual(Guid.Empty, hall.Id);
    }

    [Fact]
    public void Constructor_WithDefaultTechnology_ShouldUseStandard()
    {
        // Arrange & Act
        var hall = new CinemaHall("Hall 1");

        // Assert
        Assert.Equal(ScreeningTechnology.Standard, hall.SupportedTechnologies);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowArgumentException(string name)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CinemaHall(name));
    }

    [Fact]
    public void Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CinemaHall(null!));
    }

    #endregion

    #region Technology Management Tests

    [Fact]
    public void AddTechnology_ShouldAddNewTechnology()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);

        // Act
        hall.AddTechnology(ScreeningTechnology.ThreeD);

        // Assert
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.Standard));
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.ThreeD));
    }

    [Fact]
    public void RemoveTechnology_ShouldRemoveTechnology()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard | ScreeningTechnology.ThreeD);

        // Act
        hall.RemoveTechnology(ScreeningTechnology.ThreeD);

        // Assert
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.Standard));
        Assert.False(hall.SupportsTechnology(ScreeningTechnology.ThreeD));
    }

    [Fact]
    public void SetTechnologies_ShouldReplaceAllTechnologies()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);

        // Act
        hall.SetTechnologies(ScreeningTechnology.IMAX | ScreeningTechnology.FourDX);

        // Assert
        Assert.False(hall.SupportsTechnology(ScreeningTechnology.Standard));
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.IMAX));
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.FourDX));
    }

    [Fact]
    public void SupportsTechnology_WithSupportedTech_ShouldReturnTrue()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.IMAX);

        // Act & Assert
        Assert.True(hall.SupportsTechnology(ScreeningTechnology.IMAX));
    }

    [Fact]
    public void SupportsTechnology_WithUnsupportedTech_ShouldReturnFalse()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);

        // Act & Assert
        Assert.False(hall.SupportsTechnology(ScreeningTechnology.IMAX));
    }

    [Fact]
    public void SupportsAnyOf_WithAtLeastOneSupportedTech_ShouldReturnTrue()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard | ScreeningTechnology.ThreeD);

        // Act
        var result = hall.SupportsAnyOf(ScreeningTechnology.ThreeD, ScreeningTechnology.IMAX);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SupportsAnyOf_WithNoSupportedTech_ShouldReturnFalse()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);

        // Act
        var result = hall.SupportsAnyOf(ScreeningTechnology.IMAX, ScreeningTechnology.FourDX);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SupportsAllOf_WithAllSupportedTech_ShouldReturnTrue()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard | ScreeningTechnology.ThreeD | ScreeningTechnology.FourDX);

        // Act
        var result = hall.SupportsAllOf(ScreeningTechnology.Standard, ScreeningTechnology.ThreeD);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SupportsAllOf_WithSomeMissingTech_ShouldReturnFalse()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);

        // Act
        var result = hall.SupportsAllOf(ScreeningTechnology.Standard, ScreeningTechnology.IMAX);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowMovie_WithRequiredTechnology_ShouldReturnTrue()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.IMAX);

        // Act
        var canShow = hall.CanShowMovie(ScreeningTechnology.IMAX);

        // Assert
        Assert.True(canShow);
    }

    [Fact]
    public void CanShowMovie_WithoutRequiredTechnology_ShouldReturnFalse()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);

        // Act
        var canShow = hall.CanShowMovie(ScreeningTechnology.IMAX);

        // Assert
        Assert.False(canShow);
    }

    #endregion

    #region UpdateName Tests

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        var newName = "Premium Hall 1";

        // Act
        hall.UpdateName(newName);

        // Assert
        Assert.Equal(newName, hall.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateName_WithInvalidName_ShouldThrowArgumentException(string newName)
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => hall.UpdateName(newName));
    }

    [Fact]
    public void UpdateName_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => hall.UpdateName(null!));
    }

    #endregion

    #region Seat Management Tests

    [Fact]
    public void AddSeat_WithValidSeat_ShouldAddSeat()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        var seat = new Seat(new SeatPosition("A", 1), SeatType.Regular);

        // Act
        hall.AddSeat(seat);

        // Assert
        Assert.Single(hall.Seats);
        Assert.Contains(seat, hall.Seats);
        Assert.Equal(1, hall.Capacity);
    }

    [Fact]
    public void AddSeat_WithMultipleSeats_ShouldAddAllSeats()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        var seat1 = new Seat(new SeatPosition("A", 1), SeatType.Regular);
        var seat2 = new Seat(new SeatPosition("A", 2), SeatType.VIP);
        var seat3 = new Seat(new SeatPosition("B", 1), SeatType.Accessible);

        // Act
        hall.AddSeat(seat1);
        hall.AddSeat(seat2);
        hall.AddSeat(seat3);

        // Assert
        Assert.Equal(3, hall.Seats.Count);
        Assert.Equal(3, hall.Capacity);
    }

    [Fact]
    public void AddSeat_WithDuplicatePosition_ShouldThrowSeatAlreadyExistsException()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        var seat1 = new Seat(new SeatPosition("A", 1), SeatType.Regular);
        var seat2 = new Seat(new SeatPosition("A", 1), SeatType.VIP);
        hall.AddSeat(seat1);

        // Act & Assert
        Assert.Throws<SeatAlreadyExistsException>(() => hall.AddSeat(seat2));
    }

    [Fact]
    public void AddSeat_WithNullSeat_ShouldThrowArgumentNullException()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => hall.AddSeat(null!));
    }

    [Fact]
    public void RemoveSeat_WithExistingSeat_ShouldRemoveSeat()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        var seat = new Seat(new SeatPosition("A", 1), SeatType.Regular);
        hall.AddSeat(seat);

        // Act
        hall.RemoveSeat("A", 1);

        // Assert
        Assert.Empty(hall.Seats);
    }

    [Fact]
    public void RemoveSeat_WithNonExistingSeat_ShouldThrowArgumentNullException()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => hall.RemoveSeat("A", 1));
    }

    [Fact]
    public void GetSeat_WithExistingSeat_ShouldReturnSeat()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        var seat = new Seat(new SeatPosition("A", 1), SeatType.Regular);
        hall.AddSeat(seat);

        // Act
        var result = hall.GetSeat("A", 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(seat, result);
    }

    [Fact]
    public void GetSeat_WithNonExistingSeat_ShouldThrowArgumentNullException()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => hall.GetSeat("A", 1));
    }

    [Fact]
    public void HasSeat_WithExistingSeat_ShouldReturnTrue()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        var seat = new Seat(new SeatPosition("A", 1), SeatType.Regular);
        hall.AddSeat(seat);

        // Act
        var hasSeat = hall.HasSeat("A", 1);

        // Assert
        Assert.True(hasSeat);
    }

    [Fact]
    public void HasSeat_WithNonExistingSeat_ShouldReturnFalse()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");

        // Act
        var hasSeat = hall.HasSeat("A", 1);

        // Assert
        Assert.False(hasSeat);
    }

    [Fact]
    public void GetSeatsByType_ShouldReturnOnlySeatsOfSpecifiedType()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        hall.AddSeat(new Seat(new SeatPosition("A", 1), SeatType.Regular));
        hall.AddSeat(new Seat(new SeatPosition("A", 2), SeatType.VIP));
        hall.AddSeat(new Seat(new SeatPosition("A", 3), SeatType.Regular));
        hall.AddSeat(new Seat(new SeatPosition("B", 1), SeatType.Accessible));

        // Act
        var regularSeats = hall.GetSeatsByType(SeatType.Regular).ToList();
        var vipSeats = hall.GetSeatsByType(SeatType.VIP).ToList();

        // Assert
        Assert.Equal(2, regularSeats.Count);
        Assert.Single(vipSeats);
    }

    [Fact]
    public void GetSeatsByRow_ShouldReturnOnlySeatsInSpecifiedRow()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        hall.AddSeat(new Seat(new SeatPosition("A", 1), SeatType.Regular));
        hall.AddSeat(new Seat(new SeatPosition("A", 2), SeatType.Regular));
        hall.AddSeat(new Seat(new SeatPosition("B", 1), SeatType.Regular));
        hall.AddSeat(new Seat(new SeatPosition("B", 2), SeatType.VIP));

        // Act
        var rowASeats = hall.GetSeatsByRow("A").ToList();
        var rowBSeats = hall.GetSeatsByRow("B").ToList();

        // Assert
        Assert.Equal(2, rowASeats.Count);
        Assert.Equal(2, rowBSeats.Count);
    }

    [Fact]
    public void GetCapacityByType_ShouldReturnCorrectCount()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        hall.AddSeat(new Seat(new SeatPosition("A", 1), SeatType.Regular));
        hall.AddSeat(new Seat(new SeatPosition("A", 2), SeatType.VIP));
        hall.AddSeat(new Seat(new SeatPosition("A", 3), SeatType.Regular));
        hall.AddSeat(new Seat(new SeatPosition("B", 1), SeatType.VIP));

        // Act
        var regularCapacity = hall.GetCapacityByType(SeatType.Regular);
        var vipCapacity = hall.GetCapacityByType(SeatType.VIP);
        var accessibleCapacity = hall.GetCapacityByType(SeatType.Accessible);

        // Assert
        Assert.Equal(2, regularCapacity);
        Assert.Equal(2, vipCapacity);
        Assert.Equal(0, accessibleCapacity);
    }

    #endregion

    #region Operational Status Tests

    [Fact]
    public void SetOperationalStatus_ToFalse_ShouldMarkAsNonOperational()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");

        // Act
        hall.SetOperationalStatus(false);

        // Assert
        Assert.False(hall.IsOperational);
    }

    [Fact]
    public void SetOperationalStatus_ToTrue_ShouldMarkAsOperational()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        hall.SetOperationalStatus(false);

        // Act
        hall.SetOperationalStatus(true);

        // Assert
        Assert.True(hall.IsOperational);
    }

    #endregion

    #region Capacity Tests

    [Fact]
    public void Capacity_WithNoSeats_ShouldReturnZero()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");

        // Act & Assert
        Assert.Equal(0, hall.Capacity);
    }

    [Fact]
    public void Capacity_ShouldReflectNumberOfSeats()
    {
        // Arrange
        var hall = new CinemaHall("Hall 1");
        hall.AddSeat(new Seat(new SeatPosition("A", 1), SeatType.Regular));
        hall.AddSeat(new Seat(new SeatPosition("A", 2), SeatType.Regular));
        hall.AddSeat(new Seat(new SeatPosition("B", 1), SeatType.VIP));

        // Act & Assert
        Assert.Equal(3, hall.Capacity);
    }

    #endregion
}
