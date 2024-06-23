using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookingflightmvcUI.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public int FlightId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }   
        public Flight Flight { get; set; }
        public Cart Cart { get; set; }
    }
}
