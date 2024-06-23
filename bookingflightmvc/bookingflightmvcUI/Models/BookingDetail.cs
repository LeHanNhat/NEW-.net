using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookingflightmvcUI.Models
{
    [Table("BookingDetail")]
    public class BookingDetail
    {
        public int Id { get; set; }
        [Required]
        public int BookingId { get; set; }
        [Required]
        public int FlightId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        public Booking Booking { get; set; }
        public Flight Flight { get; set; }
    }
}
