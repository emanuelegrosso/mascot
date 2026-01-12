using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Boat> Boats { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingDate> BookingDates { get; set; }
    public DbSet<Skipper> Skippers { get; set; }
    public DbSet<BoatPrice> BoatPrices { get; set; }
    public DbSet<Damage> Damages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurazioni
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Boat)
            .WithMany(b => b.Bookings)
            .HasForeignKey(b => b.BoatId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingDate>()
            .HasOne(bd => bd.Booking)
            .WithMany(b => b.BookingDates)
            .HasForeignKey(bd => bd.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Skipper)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.SkipperId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<BoatPrice>()
            .HasOne(bp => bp.Boat)
            .WithMany(b => b.Prices)
            .HasForeignKey(bp => bp.BoatId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint: una barca può avere un solo prezzo per mese e tipo (per persona o fisso)
        modelBuilder.Entity<BoatPrice>()
            .HasIndex(bp => new { bp.BoatId, bp.Month, bp.IsPerPerson })
            .IsUnique();
    }
}
