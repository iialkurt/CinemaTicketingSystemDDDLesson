namespace CinemaTicketingSystem.Application.Test
{
    public class TicketIssuanceAppServiceTests : BaseIntegrationTest
    {
        protected TicketIssuanceAppServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
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