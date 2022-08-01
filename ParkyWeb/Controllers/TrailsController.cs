using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Controllers.Extensions;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Services.Interfaces;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class TrailsController : Controller
    {
        private readonly ITrailService _trailService;
        private readonly INationalParkService _nationalParkService;

        public TrailsController(ITrailService trailService, INationalParkService nationalParkService)
        {
            _trailService = trailService;
            _nationalParkService = nationalParkService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new TrailViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trails = await _trailService.GetAllAsync();
            return Json(new { data = trails });
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParkListViewModel = await _nationalParkService.GetAllAsync();
            var trailViewModel = this.GetDefaultViewModel(nationalParkListViewModel, new TrailViewModel());

            if (id is null)
                return View(trailViewModel);

            trailViewModel.Trail = await _trailService.GetAsync(id.Value);
            if (trailViewModel.Trail is null)
                return NotFound();

            return View(trailViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailUpsertViewModel trailUpsertViewModel)
        {
            var nationalParkListViewModel = await _nationalParkService.GetAllAsync();
            trailUpsertViewModel = this.GetDefaultViewModel(nationalParkListViewModel, trailUpsertViewModel.Trail);

            if (!ModelState.IsValid)
                return View(trailUpsertViewModel);

            var upserted = await this.InsertOrUpdateTrailAsync(_trailService, trailUpsertViewModel.Trail);
            if (upserted)
                return RedirectToAction(nameof(Index));

            return View(trailUpsertViewModel);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return NotFound();

            var response = await _trailService.DeleteAsync(id);

            return response
                ? Json(new { success = true, message = "Delete successful!" })
                : Json(new { success = false, message = "Delete not successful!" });
        }
    }
}