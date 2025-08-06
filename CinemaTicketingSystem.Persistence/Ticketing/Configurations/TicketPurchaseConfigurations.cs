#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace CinemaTicketingSystem.Persistence.Ticketing.Configurations;

internal class TicketPurchaseConfigurations : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        // Configure primary key and table
        builder.ToTable("TicketPurchases", "Ticketing");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        // Configure properties
        builder.Property(x => x.CustomerId);
        builder.Property(x => x.ScheduledMovieShowId);
        builder.Property(x => x.IsDiscountApplied);


        //builder.Metadata.FindNavigation(nameof(MovieTicket.TicketSales))!.SetPropertyAccessMode(
        //    PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(Purchase.TicketList))!.SetField("_ticketList");


        builder.HasMany(x => x.TicketList).WithOne(y => y.Purchase);
    }
}