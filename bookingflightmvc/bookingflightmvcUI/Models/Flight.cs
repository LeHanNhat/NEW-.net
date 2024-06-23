using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookingflightmvcUI.Models
{
    [Table("Flight")]
    public class Flight
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? FlightName { get; set; }

       
        [Required]
        public double ticketPrice { get; set; }
        public string? Image { get; set; }
        [Required]
        public int AirportId { get; set; }
        public Airport Airport { get; set; }


        public DateTime departureTime { get; set; }
        public DateTime arrivalTime { get; set; }
        public string duration { get; set; }
        public int numberOfStops { get; set; }
        
        public List<BookingDetail> BookingDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }
        public Stock Stock { get; set; }

        [NotMapped]
        public string AirportName { get; set; }
        [NotMapped]
        public int Quantity { get; set; }


    }
}
