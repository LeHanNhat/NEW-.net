using System.ComponentModel.DataAnnotations.Schema;

namespace bookingflightmvcUI.Models
{
    [Table("Stock")]
    public class Stock
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int Quantity { get; set; }

        public Flight? Flight { get; set; }
    }
}
