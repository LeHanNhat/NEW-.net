using bookingflightmvcUI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace bookingflightmvcUI.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock?> GetStockByBookId(int bookId) => await _context.Stocks.FirstOrDefaultAsync(s => s.FlightId == bookId);

        public async Task ManageStock(StockDTO stockToManage)
        {
            // if there is no stock for given book id, then add new record
            // if there is already stock for given book id, update stock's quantity
            var existingStock = await GetStockByBookId(stockToManage.FlightId);
            if (existingStock is null)
            {
                var stock = new Stock { FlightId = stockToManage.FlightId, Quantity = stockToManage.Quantity };
                _context.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from book in _context.Flights
                                join stock in _context.Stocks
                                on book.Id equals stock.FlightId
                                into book_stock
                                from bookStock in book_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || book.FlightName.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    FlightId = book.Id,
                                    FlightName = book.FlightName,
                                    Quantity = bookStock == null ? 0 : bookStock.Quantity
                                }
                                ).ToListAsync();
            return stocks;
        }

    }

    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByBookId(int bookId);
        Task ManageStock(StockDTO stockToManage);
    }
}
