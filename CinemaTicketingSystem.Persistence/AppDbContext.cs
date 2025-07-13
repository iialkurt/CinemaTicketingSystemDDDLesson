using Microsoft.EntityFrameworkCore;

namespace CinemaTicketingSystem.Persistence;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        foreach (var mutableProperty in entityType.GetProperties())
        {
            if (!ReferenceEquals(mutableProperty.ClrType, typeof(decimal))) continue;
            mutableProperty.SetPrecision(9);
            mutableProperty.SetScale(2);
        }


        base.OnModelCreating(modelBuilder);
    }
}