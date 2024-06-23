using bookingflightmvcUI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace bookingflightmvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sterm = "", int genreId = 0)
        {
            IEnumerable<Flight> books = await _homeRepository.GetFlights(sterm, genreId);
            IEnumerable<Airport> genres = await _homeRepository.Airports();
            FlightDisplayModel bookModel = new FlightDisplayModel
            {
                Flights = books,
                Airports = genres,
                STerm = sterm,
                AirportId = genreId
            };
            return View(bookModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
