namespace bookingflightmvcUI.Models.DTOs
{
    public class FlightDisplayModel
    {
        public IEnumerable<Flight> Flights { get; set; }
        public IEnumerable<Airport> Airports { get; set; }
        public string STerm { get; set; } = "";
        public int AirportId { get; set; } = 0;
    }
}
