using Microsoft.EntityFrameworkCore;

namespace bookingflightmvcUI.Repository
{
    public interface IFlightRepository
    {
        Task AddFlight(Flight flight);
        Task DeleteFlight(Flight flight);
        Task<Flight?> GetFlightById(int id);
        Task<IEnumerable<Flight>> GetFlights();
        Task UpdateFlight(Flight flight);
    }

    public class FlightRepository : IFlightRepository

    {
        private readonly ApplicationDbContext _context;
        public FlightRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddFlight(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFlight(Flight flight)
        {
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFlight(Flight flight)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }

        public async Task<Flight?> GetFlightById(int id) => await _context.Flights.FindAsync(id);

        public async Task<IEnumerable<Flight>> GetFlights() => await _context.Flights.Include(a=>a.Airport).ToListAsync();
    }
}
