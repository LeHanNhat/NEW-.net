using Microsoft.EntityFrameworkCore;

namespace bookingflightmvcUI.Repository;

public interface IAirportRepository
{
    Task AddAirport(Airport airport);
    Task UpdateAirport(Airport airport);
    Task<Airport?> GetAirportById(int id);
    Task DeleteAirport(Airport airport);
    Task<IEnumerable<Airport>> GetAirports();
}
public class AirportRepository : IAirportRepository
{
    private readonly ApplicationDbContext _context;
    public AirportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAirport(Airport airport)
    {
        _context.Airports.Add(airport);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAirport(Airport airport)
    {
        _context.Airports.Update(airport);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAirport(Airport airport)
    {
        _context.Airports.Remove(airport);
        await _context.SaveChangesAsync();
    }

    public async Task<Airport?> GetAirportById(int id)
    {
        return await _context.Airports.FindAsync(id);
    }

    public async Task<IEnumerable<Airport>> GetAirports()
    {
        return await _context.Airports.ToListAsync();
    }

    
}
