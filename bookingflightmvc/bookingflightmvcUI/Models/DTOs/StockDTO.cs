using System.ComponentModel.DataAnnotations;

namespace bookingflightmvcUI.Models.DTOs
{
    public class StockDTO
    {
        public int FlightId { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public int Quantity { get; set; }
    }
}
