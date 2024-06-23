using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace bookingflightmvcUI.Models.DTOs;

public class UpdateOrderStatusModel
{
    public int BookingId { get; set; }

    [Required]
    public int BookingStatusId { get; set; }

    public IEnumerable<SelectListItem>? BookingStatusList { get; set; }
}
