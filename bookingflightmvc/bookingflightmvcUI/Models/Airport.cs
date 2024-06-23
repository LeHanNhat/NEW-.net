using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bookingflightmvcUI.Models
{
    [Table("Airport")]
    public class Airport
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string AirportName { get; set; }
        public List<Flight> Flights { get; set; }
    }
}
