using bookingflightmvcUI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookingflightmvcUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AirportController : Controller
    {
        private readonly IAirportRepository _genreRepo;

        public AirportController(IAirportRepository genreRepo)
        {
            _genreRepo = genreRepo;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepo.GetAirports();
            return View(genres);
        }

        public IActionResult AddAirport()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAirport(AirportDTO genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }
            try
            {
                var genreToAdd = new Airport { AirportName = genre.AirportName, Id = genre.Id };
                await _genreRepo.AddAirport(genreToAdd);
                TempData["successMessage"] = "Genre added successfully";
                return RedirectToAction(nameof(AddAirport));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Genre could not added!";
                return View(genre);
            }

        }

        public async Task<IActionResult> UpdateAirport(int id)
        {
            var genre = await _genreRepo.GetAirportById(id);
            if (genre is null)
                throw new InvalidOperationException($"Genre with id: {id} does not found");
            var genreToUpdate = new AirportDTO
            {
                Id = genre.Id,
                AirportName = genre.AirportName
            };
            return View(genreToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAirport(AirportDTO genreToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(genreToUpdate);
            }
            try
            {
                var genre = new Airport { AirportName = genreToUpdate.AirportName, Id = genreToUpdate.Id };
                await _genreRepo.UpdateAirport(genre);
                TempData["successMessage"] = "Genre is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Genre could not updated!";
                return View(genreToUpdate);
            }

        }

        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreRepo.GetAirportById(id);
            if (genre is null)
                throw new InvalidOperationException($"Genre with id: {id} does not found");
            await _genreRepo.DeleteAirport(genre);
            return RedirectToAction(nameof(Index));

        }

    }
}
