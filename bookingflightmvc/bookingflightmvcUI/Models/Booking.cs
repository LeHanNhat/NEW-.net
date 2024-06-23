using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookingflightmvcUI.Models
{
    [Table("Booking")]
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int BookingStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string? Email { get; set; }
        [Required]
        public string? MobileNumber { get; set; }
       
        [Required]
        [MaxLength(30)]
        public string? PaymentMethod { get; set; }
        public bool IsPaid { get; set; }

        public BookingStatus BookingStatus { get; set; }
        public List<BookingDetail> BookingDetails { get; set; }
    }
}
