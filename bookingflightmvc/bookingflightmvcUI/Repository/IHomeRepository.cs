namespace bookingflightmvcUI
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Flight>> GetFlights(string sTerm = "", int genreId = 0);
        Task<IEnumerable<Airport>> Airports();
    }
}