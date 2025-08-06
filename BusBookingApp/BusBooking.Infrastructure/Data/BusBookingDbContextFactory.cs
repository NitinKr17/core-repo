using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BusBooking.Infrastructure.Data;

public class BusBookingDbContextFactory : IDesignTimeDbContextFactory<BusBookingDbContext>
{
    public BusBookingDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "BusBooking.API"))
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<BusBookingDbContext>();
        var connectionString = configuration.GetConnectionString("BusBookingDB");

        optionsBuilder.UseSqlServer(connectionString);

        return new BusBookingDbContext(optionsBuilder.Options);
    }
}
