using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookingflightmvcUI.Models
{
    [Table("BookingStatus")]
    public class BookingStatus
    {
        public int Id { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required,MaxLength(20)]
        public string ?StatusName { get; set; }
    }
}
