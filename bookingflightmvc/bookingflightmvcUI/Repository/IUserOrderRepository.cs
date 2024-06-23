using bookingflightmvcUI.Models.DTOs;

namespace bookingflightmvcUI.Repository;

public interface IUserOrderRepository
{
    Task<IEnumerable<Booking>> UserOrders(bool getAll=false);
    Task ChangeOrderStatus(UpdateOrderStatusModel data);
    Task TogglePaymentStatus(int orderId);
    Task<Booking?> GetOrderById(int id);
    Task<IEnumerable<BookingStatus>> GetOrderStatuses();

}