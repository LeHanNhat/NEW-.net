namespace bookingflightmvcUI.Models.DTOs;

public class BookingDetailModalDTO
{
    public string DivId { get; set; }
    public IEnumerable<BookingDetail> BookingDetail { get; set; }
}
