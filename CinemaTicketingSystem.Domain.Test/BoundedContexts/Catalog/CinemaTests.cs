using System;
using System.Linq;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Xunit;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.UnitTests;

public class CinemaTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateCinema()
    {
        // Arrange
        var name = "Galaxy Cinema";
        var address = new Address("Turkey", "Istanbul", "Kad?köy", "Main Street 123", "34000", "Near metro");

        // Act
        var cinema = new Cinema(name, address);

        // Assert
        Assert.NotNull(cinema);
        Assert.Equal(name, cinema.Name);
        Assert.Equal(address, cinema.Address);
        Assert.NotEqual(Guid.Empty, cinema.Id);
        Assert.Empty(cinema.Halls);
    }

    [Fact]
    public void Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var address = new Address("Turkey", "Istanbul", "Kad?köy", "Main Street 123", "34000", "Near metro");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Cinema(null!, address));
    }

    [Fact]
    public void Constructor_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var address = new Address("Turkey", "Istanbul", "Kad?köy", "Main Street 123", "34000", "Near metro");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Cinema(string.Empty, address));
    }

    [Fact]
    public void Constructor_WithWhitespaceName_ShouldThrowArgumentException()
    {
        // Arrange
        var address = new Address("Turkey", "Istanbul", "Kad?köy", "Main Street 123", "34000", "Near metro");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Cinema("   ", address));
    }

    [Fact]
    public void Constructor_WithNullAddress_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Cinema("Galaxy Cinema", null!));
    }

    #endregion

    #region UpdateName Tests

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateCinemaName()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var newName = "Updated Cinema Name";

        // Act
        cinema.UpdateName(newName);

        // Assert
        Assert.Equal(newName, cinema.Name);
    }

    [Fact]
    public void UpdateName_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var cinema = CreateValidCinema();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => cinema.UpdateName(null!));
    }

    [Fact]
    public void UpdateName_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var cinema = CreateValidCinema();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => cinema.UpdateName(string.Empty));
    }

    #endregion

    #region UpdateAddress Tests

    [Fact]
    public void UpdateAddress_WithValidAddress_ShouldUpdateCinemaAddress()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var newAddress = new Address("Turkey", "Ankara", "Çankaya", "New Street 456", "06000", "Downtown");

        // Act
        cinema.UpdateAddress(newAddress);

        // Assert
        Assert.Equal(newAddress, cinema.Address);
    }

    [Fact]
    public void UpdateAddress_WithNullAddress_ShouldThrowArgumentNullException()
    {
        // Arrange
        var cinema = CreateValidCinema();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => cinema.UpdateAddress(null!));
    }

    #endregion

    #region AddHall Tests

    [Fact]
    public void AddHall_WithValidHall_ShouldAddHallToCinema()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);

        // Act
        cinema.AddHall(hall);

        // Assert
        Assert.Single(cinema.Halls);
        Assert.Contains(hall, cinema.Halls);
    }

    [Fact]
    public void AddHall_WithMultipleHalls_ShouldAddAllHalls()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var hall1 = new CinemaHall("Hall 1", ScreeningTechnology.Standard);
        var hall2 = new CinemaHall("Hall 2", ScreeningTechnology.IMAX);
        var hall3 = new CinemaHall("Hall 3", ScreeningTechnology.ThreeD);

        // Act
        cinema.AddHall(hall1);
        cinema.AddHall(hall2);
        cinema.AddHall(hall3);

        // Assert
        Assert.Equal(3, cinema.Halls.Count);
    }

    [Fact]
    public void AddHall_WithDuplicateName_ShouldThrowCinemaHallAlreadyExistsException()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var hall1 = new CinemaHall("Hall 1", ScreeningTechnology.Standard);
        var hall2 = new CinemaHall("Hall 1", ScreeningTechnology.IMAX);
        cinema.AddHall(hall1);

        // Act & Assert
        Assert.Throws<CinemaHallAlreadyExistsException>(() => cinema.AddHall(hall2));
    }

    [Fact]
    public void AddHall_WithNullHall_ShouldThrowArgumentNullException()
    {
        // Arrange
        var cinema = CreateValidCinema();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => cinema.AddHall(null!));
    }

    [Fact]
    public void AddHall_ShouldRaiseDomainEvent()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);

        // Act
        cinema.AddHall(hall);

        // Assert
        Assert.NotEmpty(cinema.DomainEvents);
        Assert.Contains(cinema.DomainEvents, e => e.GetType().Name == "CinemaHallCreatedEvent");
    }

    #endregion

    #region RemoveHall Tests

    [Fact]
    public void RemoveHall_WithExistingHall_ShouldRemoveHall()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);
        cinema.AddHall(hall);

        // Act
        cinema.RemoveHall(hall.Id);

        // Assert
        Assert.Empty(cinema.Halls);
    }

    [Fact]
    public void RemoveHall_WithNonExistingHall_ShouldThrowCinemaHallNotFoundException()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var nonExistingHallId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<CinemaHallNotFoundException>(() => cinema.RemoveHall(nonExistingHallId));
    }

    #endregion

    #region GetHall Tests

    [Fact]
    public void GetHall_WithExistingHallId_ShouldReturnHall()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var hall = new CinemaHall("Hall 1", ScreeningTechnology.Standard);
        cinema.AddHall(hall);

        // Act
        var result = cinema.GetHall(hall.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hall, result);
    }

    [Fact]
    public void GetHall_WithNonExistingHallId_ShouldReturnNull()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var nonExistingHallId = Guid.NewGuid();

        // Act
        var result = cinema.GetHall(nonExistingHallId);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetTotalCapacity Tests

    [Fact]
    public void GetTotalCapacity_WithNoHalls_ShouldReturnZero()
    {
        // Arrange
        var cinema = CreateValidCinema();

        // Act
        var capacity = cinema.GetTotalCapacity();

        // Assert
        Assert.Equal(0, capacity);
    }

    [Fact]
    public void GetTotalCapacity_WithHallsHavingSeats_ShouldReturnTotalCapacity()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var hall1 = new CinemaHall("Hall 1", ScreeningTechnology.Standard);
        hall1.AddSeat(new Seat(new SeatPosition("A", 1), SeatType.Regular));
        hall1.AddSeat(new Seat(new SeatPosition("A", 2), SeatType.Regular));
        
        var hall2 = new CinemaHall("Hall 2", ScreeningTechnology.IMAX);
        hall2.AddSeat(new Seat(new SeatPosition("A", 1), SeatType.VIP));
        hall2.AddSeat(new Seat(new SeatPosition("A", 2), SeatType.VIP));
        hall2.AddSeat(new Seat(new SeatPosition("A", 3), SeatType.VIP));

        cinema.AddHall(hall1);
        cinema.AddHall(hall2);

        // Act
        var capacity = cinema.GetTotalCapacity();

        // Assert
        Assert.Equal(5, capacity);
    }

    #endregion

    #region GetAvailableHalls Tests

    [Fact]
    public void GetAvailableHalls_WithAllOperationalHalls_ShouldReturnAllHalls()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var hall1 = new CinemaHall("Hall 1", ScreeningTechnology.Standard);
        var hall2 = new CinemaHall("Hall 2", ScreeningTechnology.IMAX);
        cinema.AddHall(hall1);
        cinema.AddHall(hall2);

        // Act
        var availableHalls = cinema.GetAvailableHalls().ToList();

        // Assert
        Assert.Equal(2, availableHalls.Count);
    }

    [Fact]
    public void GetAvailableHalls_WithSomeNonOperationalHalls_ShouldReturnOnlyOperationalHalls()
    {
        // Arrange
        var cinema = CreateValidCinema();
        var hall1 = new CinemaHall("Hall 1", ScreeningTechnology.Standard);
        var hall2 = new CinemaHall("Hall 2", ScreeningTechnology.IMAX);
        hall2.SetOperationalStatus(false);
        
        cinema.AddHall(hall1);
        cinema.AddHall(hall2);

        // Act
        var availableHalls = cinema.GetAvailableHalls().ToList();

        // Assert
        Assert.Single(availableHalls);
        Assert.Contains(hall1, availableHalls);
        Assert.DoesNotContain(hall2, availableHalls);
    }

    #endregion

    #region Helper Methods

    private Cinema CreateValidCinema()
    {
        var address = new Address("Turkey", "Istanbul", "Kad?köy", "Main Street 123", "34000", "Near metro");
        return new Cinema("Galaxy Cinema", address);
    }

    #endregion
}
