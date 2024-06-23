

using Microsoft.EntityFrameworkCore;

namespace bookingflightmvcUI.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Airport>> Airports()
        {
            return await _db.Airports.ToListAsync();
        }
        public async Task<IEnumerable<Flight>> GetFlights(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Flight> books = await (from book in _db.Flights
                         join genre in _db.Airports
                         on book.AirportId equals genre.Id
                         join stock in _db.Stocks
                         on book.Id equals stock.FlightId
                         into book_stocks
                         from bookWithStock in book_stocks.DefaultIfEmpty()
                         where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.FlightName.ToLower().StartsWith(sTerm))
                         select new Flight
                         {
                             Id = book.Id,
                             Image = book.Image,
                             FlightName = book.FlightName,
                             ticketPrice = book.ticketPrice,
                             AirportId = book.AirportId,
                             departureTime = book.departureTime,
                             duration = book.duration,
                             numberOfStops = book.numberOfStops,
                             arrivalTime = book.arrivalTime,
                             AirportName = genre.AirportName,
                             Quantity=bookWithStock==null? 0:bookWithStock.Quantity
                         }
                         ).ToListAsync();
            if (genreId > 0)
            {

                books = books.Where(a => a.AirportId == genreId).ToList();
            }
            return books;

        }
    }
}
