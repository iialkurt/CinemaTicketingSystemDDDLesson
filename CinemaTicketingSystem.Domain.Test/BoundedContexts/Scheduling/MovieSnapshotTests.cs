
using System;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Xunit;


namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.UnitTests;

public class MovieSnapshotTests
{
    /// <summary>
    /// Helper class to expose the protected parameterless constructor for testing purposes.
    /// </summary>
    private class TestableMovieSnapshot : MovieSnapshot
    {
        public TestableMovieSnapshot() : base()
        {
        }
    }

    /// <summary>
    /// Verifies that the protected parameterless constructor successfully creates an instance
    /// with default property values.
    /// </summary>
    [Fact]
    public void Constructor_ShouldCreateInstance_WithDefaultValues()
    {
        // Arrange & Act
        TestableMovieSnapshot movieSnapshot = new TestableMovieSnapshot();

        // Assert
        Assert.Equal(Guid.Empty, movieSnapshot.Id);
        Assert.Null(movieSnapshot.Duration);
        Assert.Equal(ScreeningTechnology.Standard, movieSnapshot.SupportedTechnology);
    }
}