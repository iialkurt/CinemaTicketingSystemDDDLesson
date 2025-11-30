using CinemaTicketingSystem.Application.Catalog.Movie;
using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.Catalog.Movie;
using CinemaTicketingSystem.Application.Contracts.Catalog.Movie.Create;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CinemaTicketingSystem.Application.Test;

public class MovieAppServiceTests : BaseIntegrationTest
{
    private readonly IMovieAppService _movieAppService;

    public MovieAppServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _movieAppService = GetService<IMovieAppService>();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateMovie()
    {
        // Arrange
        var request = new CreateMovieRequest(
            "Test Movie",
            "https://example.com/poster.jpg",
            "Test Original",
            "A great test movie",
            TimeSpan.FromMinutes(120),
            DateTime.Today.AddDays(7));

        // Act
        var result = await _movieAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotEqual(Guid.Empty, result.Data.NewMovieId);

        var movie = await DbContext.Movies.FindAsync(result.Data.NewMovieId);
        Assert.NotNull(movie);
        Assert.Equal("Test Movie", movie.Title);
        Assert.Equal("Test Original", movie.OriginalTitle);
        Assert.Equal("A great test movie", movie.Description);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateTitle_ShouldReturnError()
    {
        // Arrange
        var existingMovie = new Domain.BoundedContexts.Catalog.Movie(
            "Existing Movie",
            new Duration(120),
            "https://example.com/poster.jpg");
        
        await DbContext.Movies.AddAsync(existingMovie);
        await DbContext.SaveChangesAsync();

        var request = new CreateMovieRequest(
            "Existing Movie",
            "https://example.com/new-poster.jpg",
            null,
            null,
            TimeSpan.FromMinutes(120),
            null);

        // Act
        var result = await _movieAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsFail);
        Assert.Contains("MovieAlreadyExists", result.ProblemDetails?.Title ?? "");
    }

    [Fact]
    public async Task CreateAsync_WithMinimalData_ShouldCreateMovie()
    {
        // Arrange
        var request = new CreateMovieRequest(
            "Minimal Movie",
            "https://example.com/minimal.jpg",
            null,
            null,
            TimeSpan.FromMinutes(90),
            null);

        // Act
        var result = await _movieAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        var movie = await DbContext.Movies.FindAsync(result.Data.NewMovieId);
        Assert.NotNull(movie);
        Assert.Equal("Minimal Movie", movie.Title);
        Assert.Null(movie.OriginalTitle);
        Assert.Null(movie.Description);
    }

    [Fact]
    public async Task CreateAsync_WithEarliestShowingDate_ShouldSetDate()
    {
        // Arrange
        var futureDate = DateTime.Today.AddDays(30);
        var request = new CreateMovieRequest(
            "Future Movie",
            "https://example.com/future.jpg",
            null,
            null,
            TimeSpan.FromMinutes(150),
            futureDate);

        // Act
        var result = await _movieAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        
        var movie = await DbContext.Movies.FindAsync(result.Data.NewMovieId);
        Assert.NotNull(movie);
        Assert.NotNull(movie.EarliestShowingDate);
        Assert.Equal(futureDate.Date, movie.EarliestShowingDate.Value.Date);
    }

    [Fact]
    public async Task GetAllAsync_WithNoMovies_ShouldReturnEmptyList()
    {
        // Arrange
        // Clear all existing movies
        var allMovies = await DbContext.Movies.ToListAsync();
        DbContext.Movies.RemoveRange(allMovies);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _movieAppService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data.Movies);
    }

    [Fact]
    public async Task GetAllAsync_WithMultipleMovies_ShouldReturnAllMovies()
    {
        // Arrange
        var movie1 = new Domain.BoundedContexts.Catalog.Movie(
            "Movie 1",
            new Duration(100),
            "https://example.com/1.jpg");
        
        var movie2 = new Domain.BoundedContexts.Catalog.Movie(
            "Movie 2",
            new Duration(120),
            "https://example.com/2.jpg");
        
        var movie3 = new Domain.BoundedContexts.Catalog.Movie(
            "Movie 3",
            new Duration(90),
            "https://example.com/3.jpg");

        await DbContext.Movies.AddRangeAsync(movie1, movie2, movie3);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _movieAppService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Movies.Count >= 3);
        Assert.Contains(result.Data.Movies, m => m.Title == "Movie 1");
        Assert.Contains(result.Data.Movies, m => m.Title == "Movie 2");
        Assert.Contains(result.Data.Movies, m => m.Title == "Movie 3");
    }

    [Fact]
    public async Task GetAllAsync_ShouldIncludeAllMovieProperties()
    {
        // Arrange
        var movie = new Domain.BoundedContexts.Catalog.Movie(
            "Complete Movie",
            new Duration(125),
            "https://example.com/complete.jpg",
            ScreeningTechnology.IMAX);
        
        movie.SetOriginalTitle("Original Complete");
        movie.SetDescription("A complete description");

        await DbContext.Movies.AddAsync(movie);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _movieAppService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        var movieDto = result.Data.Movies.FirstOrDefault(m => m.Title == "Complete Movie");
        Assert.NotNull(movieDto);
        Assert.Equal("Original Complete", movieDto.OriginalTitle);
        Assert.Equal("A complete description", movieDto.Description);
        Assert.Equal(125, movieDto.DurationMinutes);
        Assert.Contains("2 hrs 5 min", movieDto.DurationFormatted);
        Assert.Equal(ScreeningTechnology.IMAX, movieDto.SupportedTechnology);
    }

    [Theory]
    [InlineData(90, "1 hr 30 min")]
    [InlineData(120, "2 hrs")]
    [InlineData(150, "2 hrs 30 min")]
    public async Task CreateAsync_WithVariousDurations_ShouldFormatCorrectly(int minutes, string expectedFormat)
    {
        // Arrange
        var request = new CreateMovieRequest(
            $"Movie {minutes}min",
            "https://example.com/test.jpg",
            null,
            null,
            TimeSpan.FromMinutes(minutes),
            null);

        // Act
        var result = await _movieAppService.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        
        var allMovies = await _movieAppService.GetAllAsync();
        var movieDto = allMovies.Data!.Movies.FirstOrDefault(m => m.Title == $"Movie {minutes}min");
        Assert.NotNull(movieDto);
        Assert.Contains(expectedFormat, movieDto.DurationFormatted);
    }
}
