namespace CinemaTicketingSystem.Application.Test
{
    public class TicketIssuanceAppServiceTests : BaseIntegrationTest
    {
        public TicketIssuanceAppServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Test1()
        {
            var movieList = DbContext.Movies.ToList();

            Assert.Empty(movieList);
        }
    }
}