using CinemaTicketingSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.MsSql;

namespace CinemaTicketingSystem.Application.Test
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private const string Username = "sa";
        private const string Password = "yourStrong(!)Password";
        private const ushort MsSqlPort = 1438;


        private MsSqlContainer _mssqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Strong_password_123!")
            .Build();


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(_mssqlContainer.GetConnectionString()));
            });


            // base.ConfigureWebHost(builder);
        }

        public async Task InitializeAsync()
        {
            var connectionString = _mssqlContainer.GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString);

            await using var dbContext = new AppDbContext(optionsBuilder.Options, null, null);
            await dbContext.Database.MigrateAsync();


            await _mssqlContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _mssqlContainer.DisposeAsync();
        }
    }
}