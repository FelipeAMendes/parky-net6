using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Controllers.Extensions;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Services.Interfaces;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class NationalParksController : Controller
    {
        private readonly INationalParkService _nationalParkService;

        public NationalParksController(INationalParkService nationalParkService)
        {
            _nationalParkService = nationalParkService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new NationalParkViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var nationalParks = await _nationalParkService.GetAllAsync();
            return Json(new { data = nationalParks });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParkViewModel = new NationalParkViewModel();
            if (id is null)
                return View(nationalParkViewModel);

            nationalParkViewModel = await _nationalParkService.GetAsync(id.Value);
            if (nationalParkViewModel is null)
                return NotFound();

            return View(nationalParkViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(NationalParkViewModel nationalParkViewModel)
        {
            if (!ModelState.IsValid)
                return View(nationalParkViewModel);

            await this.DefinePicture(_nationalParkService,
                                     nationalParkViewModel,
                                     HttpContext.Request.Form.Files);

            var upserted = await this.InsertOrUpdateNationalPark(_nationalParkService, nationalParkViewModel);
            if (upserted)
                return RedirectToAction(nameof(Index));

            return View(nationalParkViewModel);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var response = await _nationalParkService.DeleteAsync(id);

            return response
                ? Json(new { success = true, message = "Delete successful!" })
                : Json(new { success = false, message = "Delete not successful!" });
        }
    }
}