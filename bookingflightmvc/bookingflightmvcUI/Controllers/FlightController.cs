using bookingflightmvcUI.Models.DTOs;
using bookingflightmvcUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bookingflightmvcUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class FlightController : Controller
    {
        private readonly IFlightRepository _bookRepo;
        private readonly IAirportRepository _genreRepo;
        private readonly IFileService _fileService;

        public FlightController(IFlightRepository bookRepo, IAirportRepository genreRepo, IFileService fileService)
        {
            _bookRepo = bookRepo;
            _genreRepo = genreRepo;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookRepo.GetFlights();
            return View(books);
        }

        public async Task<IActionResult> AddFlight()
        {
            var genreSelectList = (await _genreRepo.GetAirports()).Select(genre => new SelectListItem
            {
                Text = genre.AirportName,
                Value = genre.Id.ToString(),
            });
            FlightDTO bookToAdd = new() { AirportList = genreSelectList };
            return View(bookToAdd);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlight(FlightDTO bookToAdd)
        {
            var genreSelectList = (await _genreRepo.GetAirports()).Select(genre => new SelectListItem
            {
                Text = genre.AirportName,
                Value = genre.Id.ToString(),
            });
            bookToAdd.AirportList = genreSelectList;

            if (!ModelState.IsValid)
                return View(bookToAdd);

            try
            {
                if (bookToAdd.ImageFile != null)
                {
                    if (bookToAdd.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file can not exceed 1 MB");
                    }
                    string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                    string imageName = await _fileService.SaveFile(bookToAdd.ImageFile, allowedExtensions);
                    bookToAdd.Image = imageName;
                }
                // manual mapping of BookDTO -> Book
                Flight book = new()
                {
                    Id = bookToAdd.Id,
                    Image = bookToAdd.Image,
                    FlightName = bookToAdd.FlightName,
                    ticketPrice = bookToAdd.Price,
                    AirportId = bookToAdd.AirportId,
                    departureTime = bookToAdd.departureTime,
                    duration = bookToAdd.duration,
                    numberOfStops = bookToAdd.numberOfStops,
                    arrivalTime = bookToAdd.arrivalTime,


                };
                await _bookRepo.AddFlight(book);
                TempData["successMessage"] = "Flight is added successfully";
                return RedirectToAction(nameof(AddFlight));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(bookToAdd);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(bookToAdd);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error on saving data";
                return View(bookToAdd);
            }
        }

        public async Task<IActionResult> UpdateFlight(int id)
        {
            var book = await _bookRepo.GetFlightById(id);
            if (book == null)
            {
                TempData["errorMessage"] = $"Book with the id: {id} does not found";
                return RedirectToAction(nameof(Index));
            }
            var genreSelectList = (await _genreRepo.GetAirports()).Select(genre => new SelectListItem
            {
                Text = genre.AirportName,
                Value = genre.Id.ToString(),
                Selected = genre.Id == book.AirportId
            });
            FlightDTO bookToUpdate = new()
            {
                AirportList = genreSelectList,
                FlightName = book.FlightName,

                AirportId = book.AirportId,
                Price = book.ticketPrice,
                Image = book.Image,
                departureTime = book.departureTime,
                duration = book.duration,
                numberOfStops = book.numberOfStops,
                arrivalTime = book.arrivalTime,
            };
            return View(bookToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFlight(FlightDTO bookToUpdate)
        {
            var genreSelectList = (await _genreRepo.GetAirports()).Select(genre => new SelectListItem
            {
                Text = genre.AirportName,
                Value = genre.Id.ToString(),
                Selected = genre.Id == bookToUpdate.AirportId
            });
            bookToUpdate.AirportList = genreSelectList;

            if (!ModelState.IsValid)
                return View(bookToUpdate);

            try
            {
                string oldImage = "";
                if (bookToUpdate.ImageFile != null)
                {
                    if (bookToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file can not exceed 1 MB");
                    }
                    string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                    string imageName = await _fileService.SaveFile(bookToUpdate.ImageFile, allowedExtensions);
                    // hold the old image name. Because we will delete this image after updating the new
                    oldImage = bookToUpdate.Image;
                    bookToUpdate.Image = imageName;
                }
                // manual mapping of BookDTO -> Book
                Flight book = new()
                {
                    Id = bookToUpdate.Id,
                    Image = bookToUpdate.Image,
                    FlightName = bookToUpdate.FlightName,
                    ticketPrice = bookToUpdate.Price,
                    AirportId = bookToUpdate.AirportId,
                    departureTime = bookToUpdate.departureTime,
                    duration = bookToUpdate.duration,
                    numberOfStops = bookToUpdate.numberOfStops,
                    arrivalTime = bookToUpdate.arrivalTime,
                };
                await _bookRepo.UpdateFlight(book);
                // if image is updated, then delete it from the folder too
                if (!string.IsNullOrWhiteSpace(oldImage))
                {
                    _fileService.DeleteFile(oldImage);
                }
                TempData["successMessage"] = "Book is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(bookToUpdate);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(bookToUpdate);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error on saving data";
                return View(bookToUpdate);
            }
        }

        public async Task<IActionResult> DeleteFlight(int id)
        {
            try
            {
                var book = await _bookRepo.GetFlightById(id);
                if (book == null)
                {
                    TempData["errorMessage"] = $"Book with the id: {id} does not found";
                }
                else
                {
                    await _bookRepo.DeleteFlight(book);
                    if (!string.IsNullOrWhiteSpace(book.Image))
                    {
                        _fileService.DeleteFile(book.Image);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error on deleting the data";
            }
            return RedirectToAction(nameof(Index));
        }

    }

}
