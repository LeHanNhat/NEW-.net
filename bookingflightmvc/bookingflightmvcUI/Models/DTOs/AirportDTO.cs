using System.ComponentModel.DataAnnotations;

namespace bookingflightmvcUI.Models.DTOs
{
    public class AirportDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string AirportName { get; set; }
    }
}
