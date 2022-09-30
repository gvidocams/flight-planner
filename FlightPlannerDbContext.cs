using Microsoft.EntityFrameworkCore;

namespace FlightPlanner
{
    public class FlightPlannerDbContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airport> Airports { get; set; }
    }
}
