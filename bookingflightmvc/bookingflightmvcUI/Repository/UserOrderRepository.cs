using bookingflightmvcUI.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bookingflightmvcUI.Repository
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserOrderRepository(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            var order = await _db.Bookings.FindAsync(data.BookingId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{data.BookingId} does not found");
            }
            order.BookingStatusId = data.BookingStatusId;
            await _db.SaveChangesAsync();
        }

        public async Task<Booking?> GetOrderById(int id)
        {
            return await _db.Bookings.FindAsync(id);
        }

        public async Task<IEnumerable<BookingStatus>> GetOrderStatuses()
        {
            return await _db.BookingStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _db.Bookings.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{orderId} does not found");
            }
            order.IsPaid = !order.IsPaid;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> UserOrders(bool getAll = false)
        {
            var orders = _db.Bookings
                           .Include(x => x.BookingStatus)
                           .Include(x => x.BookingDetails)
                           .ThenInclude(x => x.Flight)
                           .ThenInclude(x => x.Airport).AsQueryable();
            if (!getAll)
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                orders = orders.Where(a => a.UserId == userId);
                return await orders.ToListAsync();
            }

            return await orders.ToListAsync();
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
