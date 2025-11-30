using System;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Xunit;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.UnitTests;

public class ShowTimeTests
{
    #region Create Tests - TimeOnly Overload

    [Fact]
    public void Create_WithValidTimeRange_ShouldCreateShowTime()
    {
        // Arrange
        var startTime = new TimeOnly(14, 0);
        var endTime = new TimeOnly(16, 30);

        // Act
        var showTime = ShowTime.Create(startTime, endTime);

        // Assert
        Assert.NotNull(showTime);
        Assert.Equal(startTime, showTime.StartTime);
        Assert.Equal(endTime, showTime.EndTime);
        Assert.Equal(TimeSpan.FromMinutes(150), showTime.Duration);
    }

    [Fact]
    public void Create_WhenStartTimeEqualsEndTime_ShouldThrowArgumentException()
    {
        // Arrange
        var time = new TimeOnly(14, 0);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ShowTime.Create(time, time));
        Assert.Equal("startTime", exception.ParamName);
        Assert.Contains("Start time must be before end time", exception.Message);
    }

    [Fact]
    public void Create_WhenStartTimeIsAfterEndTime_ShouldThrowArgumentException()
    {
        // Arrange
        var startTime = new TimeOnly(16, 30);
        var endTime = new TimeOnly(14, 0);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ShowTime.Create(startTime, endTime));
        Assert.Equal("startTime", exception.ParamName);
    }

    [Theory]
    [InlineData(0, 0, 2, 30, 150)] // Midnight to 2:30 AM
    [InlineData(9, 0, 11, 30, 150)] // Morning show
    [InlineData(13, 45, 16, 15, 150)] // Afternoon show
    [InlineData(19, 0, 22, 0, 180)] // Evening show
    [InlineData(22, 30, 23, 59, 89)] // Late night show
    public void Create_WithVariousValidTimeRanges_ShouldCalculateCorrectDuration(
        int startHour, int startMinute, int endHour, int endMinute, int expectedDurationMinutes)
    {
        // Arrange
        var startTime = new TimeOnly(startHour, startMinute);
        var endTime = new TimeOnly(endHour, endMinute);

        // Act
        var showTime = ShowTime.Create(startTime, endTime);

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(expectedDurationMinutes), showTime.Duration);
    }

    #endregion

    #region Create Tests - Duration Overload

    [Fact]
    public void Create_WithDuration_ShouldCreateShowTime()
    {
        // Arrange
        var startTime = new TimeOnly(14, 0);
        var duration = new Duration(120);

        // Act
        var showTime = ShowTime.Create(startTime, duration);

        // Assert
        Assert.NotNull(showTime);
        Assert.Equal(startTime, showTime.StartTime);
        Assert.Equal(new TimeOnly(16, 0), showTime.EndTime);
        Assert.Equal(TimeSpan.FromMinutes(120), showTime.Duration);
    }

    [Fact]
    public void Create_WithNullDuration_ShouldThrowArgumentNullException()
    {
        // Arrange
        var startTime = new TimeOnly(14, 0);
        Duration? duration = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => ShowTime.Create(startTime, duration!));
    }

    [Fact]
    public void Create_WithZeroDuration_ShouldThrowArgumentException()
    {
        // Arrange
        var startTime = new TimeOnly(14, 0);

        // Act & Assert
        // Duration constructor throws for zero value
        Assert.Throws<ArgumentException>(() => new Duration(0));
    }

    [Fact]
    public void Create_WithNegativeDuration_ShouldThrowArgumentException()
    {
        // Arrange
        var startTime = new TimeOnly(14, 0);

        // Act & Assert
        // Duration constructor throws for negative value
        Assert.Throws<ArgumentException>(() => new Duration(-60));
    }

    [Theory]
    [InlineData(9, 0, 90, 10, 30)]
    [InlineData(14, 30, 120, 16, 30)]
    [InlineData(19, 0, 150, 21, 30)]
    public void Create_WithDuration_ShouldCalculateCorrectEndTime(
        int startHour, int startMinute, int durationMinutes, int expectedEndHour, int expectedEndMinute)
    {
        // Arrange
        var startTime = new TimeOnly(startHour, startMinute);
        var duration = new Duration(durationMinutes);
        var expectedEndTime = new TimeOnly(expectedEndHour, expectedEndMinute);

        // Act
        var showTime = ShowTime.Create(startTime, duration);

        // Assert
        Assert.Equal(expectedEndTime, showTime.EndTime);
    }

    #endregion

    #region ConflictsWith Tests

    [Fact]
    public void ConflictsWith_WhenShowTimesOverlap_ShouldReturnTrue()
    {
        // Arrange
        var showTime1 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(15, 0), new TimeOnly(17, 0));

        // Act
        var conflicts = showTime1.ConflictsWith(showTime2);

        // Assert
        Assert.True(conflicts);
    }

    [Fact]
    public void ConflictsWith_WhenShowTimesDoNotOverlap_ShouldReturnFalse()
    {
        // Arrange
        var showTime1 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(17, 0), new TimeOnly(19, 0));

        // Act
        var conflicts = showTime1.ConflictsWith(showTime2);

        // Assert
        Assert.False(conflicts);
    }

    [Fact]
    public void ConflictsWith_WithCleaningTime_ShouldConsiderCleaningPeriod()
    {
        // Arrange
        var showTime1 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(16, 15), new TimeOnly(18, 0));

        // Act
        var conflictsWithCleaning = showTime1.ConflictsWith(showTime2, cleaningTime: 30);
        var conflictsWithoutCleaning = showTime1.ConflictsWith(showTime2, cleaningTime: 0);

        // Assert
        Assert.True(conflictsWithCleaning, "Should conflict when cleaning time is considered");
        Assert.False(conflictsWithoutCleaning, "Should not conflict without cleaning time");
    }

    [Fact]
    public void ConflictsWith_WhenOtherIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => showTime.ConflictsWith(null!));
    }

    [Fact]
    public void ConflictsWith_WithNegativeCleaningTime_ShouldThrowArgumentException()
    {
        // Arrange
        var showTime1 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(17, 0), new TimeOnly(19, 0));

        // Act & Assert
        Assert.Throws<ArgumentException>(() => showTime1.ConflictsWith(showTime2, cleaningTime: -10));
    }

    [Fact]
    public void ConflictsWith_WhenSecondShowStartsExactlyWhenFirstEndsWithCleaning_ShouldNotConflict()
    {
        // Arrange
        var showTime1 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(16, 30), new TimeOnly(18, 0));
        int cleaningTime = 30;

        // Act
        var conflicts = showTime1.ConflictsWith(showTime2, cleaningTime);

        // Assert
        Assert.False(conflicts);
    }

    #endregion

    #region IsMorningShow Tests

    [Theory]
    [InlineData(9, 0, true)]
    [InlineData(10, 30, true)]
    [InlineData(11, 59, true)]
    public void IsMorningShow_WhenStartTimeBetween9And12_ShouldReturnTrue(int hour, int minute, bool expected)
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(hour, minute), new TimeOnly(hour + 2, minute));

        // Act
        var isMorning = showTime.IsMorningShow();

        // Assert
        Assert.Equal(expected, isMorning);
    }

    [Theory]
    [InlineData(8, 59, false)]
    [InlineData(12, 0, false)]
    [InlineData(14, 0, false)]
    [InlineData(19, 0, false)]
    public void IsMorningShow_WhenStartTimeOutsideMorningRange_ShouldReturnFalse(int hour, int minute, bool expected)
    {
        // Arrange
        var endHour = hour + 2;
        if (endHour > 23) endHour = 23;
        var showTime = ShowTime.Create(new TimeOnly(hour, minute), new TimeOnly(endHour, 59));

        // Act
        var isMorning = showTime.IsMorningShow();

        // Assert
        Assert.Equal(expected, isMorning);
    }

    #endregion

    #region IsSuitableForChildren Tests

    [Theory]
    [InlineData(10, 0, true)]
    [InlineData(14, 30, true)]
    [InlineData(20, 0, true)]
    public void IsSuitableForChildren_WhenStartTimeBetween10And20_ShouldReturnTrue(int hour, int minute, bool expected)
    {
        // Arrange
        var endHour = hour + 2;
        if (endHour > 23) endHour = 23;
        var showTime = ShowTime.Create(new TimeOnly(hour, minute), new TimeOnly(endHour, 0));

        // Act
        var isSuitable = showTime.IsSuitableForChildren();

        // Assert
        Assert.Equal(expected, isSuitable);
    }

    [Theory]
    [InlineData(9, 59, false)]
    [InlineData(21, 0, false)]
    [InlineData(22, 30, false)]
    public void IsSuitableForChildren_WhenStartTimeOutsideChildrenRange_ShouldReturnFalse(int hour, int minute, bool expected)
    {
        // Arrange
        var endHour = hour + 2;
        if (endHour > 23) endHour = 23;
        var showTime = ShowTime.Create(new TimeOnly(hour, minute), new TimeOnly(endHour, 59));

        // Act
        var isSuitable = showTime.IsSuitableForChildren();

        // Assert
        Assert.Equal(expected, isSuitable);
    }

    #endregion

    #region HasStarted Tests

    [Fact]
    public void HasStarted_WhenCurrentTimeIsBeforeStartTime_ShouldReturnFalse()
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var currentTime = new TimeOnly(13, 59);

        // Act
        var hasStarted = showTime.HasStarted(currentTime);

        // Assert
        Assert.False(hasStarted);
    }

    [Fact]
    public void HasStarted_WhenCurrentTimeIsExactlyStartTime_ShouldReturnTrue()
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var currentTime = new TimeOnly(14, 0);

        // Act
        var hasStarted = showTime.HasStarted(currentTime);

        // Assert
        Assert.True(hasStarted);
    }

    [Fact]
    public void HasStarted_WhenCurrentTimeIsAfterStartTime_ShouldReturnTrue()
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var currentTime = new TimeOnly(15, 30);

        // Act
        var hasStarted = showTime.HasStarted(currentTime);

        // Assert
        Assert.True(hasStarted);
    }

    #endregion

    #region HasEnded Tests

    [Fact]
    public void HasEnded_WhenCurrentTimeIsBeforeEndTime_ShouldReturnFalse()
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var currentTime = new TimeOnly(15, 59);

        // Act
        var hasEnded = showTime.HasEnded(currentTime);

        // Assert
        Assert.False(hasEnded);
    }

    [Fact]
    public void HasEnded_WhenCurrentTimeIsExactlyEndTime_ShouldReturnTrue()
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var currentTime = new TimeOnly(16, 0);

        // Act
        var hasEnded = showTime.HasEnded(currentTime);

        // Assert
        Assert.True(hasEnded);
    }

    [Fact]
    public void HasEnded_WhenCurrentTimeIsAfterEndTime_ShouldReturnTrue()
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var currentTime = new TimeOnly(17, 0);

        // Act
        var hasEnded = showTime.HasEnded(currentTime);

        // Assert
        Assert.True(hasEnded);
    }

    #endregion

    #region IsShortShow Tests

    [Theory]
    [InlineData(60, true)]
    [InlineData(90, true)]
    [InlineData(89, true)]
    public void IsShortShow_WhenDurationIs90MinutesOrLess_ShouldReturnTrue(int durationMinutes, bool expected)
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new Duration(durationMinutes));

        // Act
        var isShort = showTime.IsShortShow();

        // Assert
        Assert.Equal(expected, isShort);
    }

    [Theory]
    [InlineData(91, false)]
    [InlineData(120, false)]
    [InlineData(180, false)]
    public void IsShortShow_WhenDurationIsMoreThan90Minutes_ShouldReturnFalse(int durationMinutes, bool expected)
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new Duration(durationMinutes));

        // Act
        var isShort = showTime.IsShortShow();

        // Assert
        Assert.Equal(expected, isShort);
    }

    #endregion

    #region IsLongShow Tests

    [Theory]
    [InlineData(180, true)]
    [InlineData(200, true)]
    [InlineData(240, true)]
    public void IsLongShow_WhenDurationIs180MinutesOrMore_ShouldReturnTrue(int durationMinutes, bool expected)
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new Duration(durationMinutes));

        // Act
        var isLong = showTime.IsLongShow();

        // Assert
        Assert.Equal(expected, isLong);
    }

    [Theory]
    [InlineData(179, false)]
    [InlineData(120, false)]
    [InlineData(90, false)]
    public void IsLongShow_WhenDurationIsLessThan180Minutes_ShouldReturnFalse(int durationMinutes, bool expected)
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 0), new Duration(durationMinutes));

        // Act
        var isLong = showTime.IsLongShow();

        // Assert
        Assert.Equal(expected, isLong);
    }

    #endregion

    #region GetDisplayInfo Tests

    [Fact]
    public void GetDisplayInfo_ShouldReturnFormattedString()
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(14, 30), new TimeOnly(16, 30));

        // Act
        var displayInfo = showTime.GetDisplayInfo();

        // Assert
        Assert.Equal("14:30 - 16:30 (120 min)", displayInfo);
    }

    [Theory]
    [InlineData(9, 0, 11, 30, "09:00 - 11:30 (150 min)")]
    [InlineData(14, 15, 16, 45, "14:15 - 16:45 (150 min)")]
    [InlineData(19, 0, 22, 0, "19:00 - 22:00 (180 min)")]
    public void GetDisplayInfo_WithVariousTimes_ShouldReturnCorrectFormat(
        int startHour, int startMinute, int endHour, int endMinute, string expected)
    {
        // Arrange
        var showTime = ShowTime.Create(new TimeOnly(startHour, startMinute), new TimeOnly(endHour, endMinute));

        // Act
        var displayInfo = showTime.GetDisplayInfo();

        // Assert
        Assert.Equal(expected, displayInfo);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WhenShowTimesHaveSameStartAndEndTime_ShouldReturnTrue()
    {
        // Arrange
        var showTime1 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));

        // Act & Assert
        Assert.Equal(showTime1, showTime2);
        Assert.True(showTime1 == showTime2);
    }

    [Fact]
    public void Equals_WhenShowTimesHaveDifferentStartTime_ShouldReturnFalse()
    {
        // Arrange
        var showTime1 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(15, 0), new TimeOnly(16, 0));

        // Act & Assert
        Assert.NotEqual(showTime1, showTime2);
        Assert.True(showTime1 != showTime2);
    }

    [Fact]
    public void Equals_WhenShowTimesHaveDifferentEndTime_ShouldReturnFalse()
    {
        // Arrange
        var showTime1 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(17, 0));

        // Act & Assert
        Assert.NotEqual(showTime1, showTime2);
    }

    [Fact]
    public void GetHashCode_WhenShowTimesAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var showTime1 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));
        var showTime2 = ShowTime.Create(new TimeOnly(14, 0), new TimeOnly(16, 0));

        // Act & Assert
        Assert.Equal(showTime1.GetHashCode(), showTime2.GetHashCode());
    }

    #endregion
}
