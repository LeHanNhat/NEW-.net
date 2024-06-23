using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace bookingflightmvcUI.Models.DTOs;
public class FlightDTO
{
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string? FlightName { get; set; }

    
    [Required]
    public double Price { get; set; }
    public string? Image { get; set; }
    [Required]
    public int AirportId { get; set; }
    public DateTime departureTime { get; set; }
    public DateTime arrivalTime { get; set; }
    public string duration { get; set; }
    public int numberOfStops { get; set; }
    public IFormFile? ImageFile { get; set; }
    public IEnumerable<SelectListItem>? AirportList { get; set; }
}
