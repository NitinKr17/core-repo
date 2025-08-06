using BusBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Infrastructure.Data;

public class BusBookingDbContext : DbContext
{
    public BusBookingDbContext(DbContextOptions<BusBookingDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Bus> Buses => Set<Bus>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Passenger> Passengers => Set<Passenger>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>()
            .HasMany(b => b.Passengers)
            .WithOne(p => p.Booking)
            .HasForeignKey(p => p.BookingId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Bus)
            .WithMany(bus => bus.Bookings)
            .HasForeignKey(b => b.BusId);
    }
}

