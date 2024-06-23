namespace bookingflightmvcUI.Models.DTOs
{
    public class StockDisplayModel
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int Quantity { get; set; }
        public string? FlightName { get; set; }
    }
}
