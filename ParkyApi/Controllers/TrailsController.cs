using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Filters;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Models.Interfaces;
using ParkyApi.Repositories.Interfaces;

namespace ParkyApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "Admin")]
//[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrail")]
public class TrailsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;
    private readonly INationalParkRepository _nationalParkRepository;

    public TrailsController(IMapper mapper, ITrailRepository trailRepository, INationalParkRepository nationalParkRepository)
    {
        _mapper = mapper;
        _trailRepository = trailRepository;
        _nationalParkRepository = nationalParkRepository;
    }

    /// <summary>
    /// Get list of the trails
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync()
    {
        var model = await _trailRepository.GetAllAsync();

        var result = _mapper.Map<List<TrailDto>>(model);

        return new JsonResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var model = await _trailRepository.GetByIdAsync(id);
        if (model is null) return NotFound();

        var result = _mapper.Map<TrailDto>(model);

        return new JsonResult(result);
    }

    [HttpGet("nationalPark/{id:int}")]
    public async Task<IActionResult> GetInNationalParksAsync(int id)
    {
        if (id == 0) return NotFound();

        var model = await _trailRepository.GetTrailsInNationalParkAsync(id);

        var result = _mapper.Map<List<TrailDto>>(model);

        return new JsonResult(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(TransactionActionFilter))]
    public async Task<IApiResult> PostAsync([FromBody] TrailCreateDto trailCreateDto)
    {
        if (!ModelState.IsValid)
            return new ApiResult(false, ModelState);

        var modelExists = await _trailRepository.TrailExistsAsync(x => x.Name == trailCreateDto.Name);
        if (modelExists)
        {
            ModelState.AddModelError("ModelError", "Trail already exists!");
            return new ApiResult(false, ModelState);
        }

        var nationalPark = await _nationalParkRepository.GetByIdAsync(trailCreateDto.NationalParkId);
        if (nationalPark is null)
        {
            ModelState.AddModelError("ModelError", $"National Park {trailCreateDto.NationalParkId} does not exist!");
            return new ApiResult(false, ModelState);
        }

        var model = _mapper.Map<Trail>(trailCreateDto);
        model.HasCreated();
        model.DefineNationalPark(nationalPark);
        var modelCreated = await _trailRepository.CreateAsync(model);

        var resultDto = _mapper.Map<TrailDto>(modelCreated);

        return new ApiResult(resultDto);
    }

    [HttpPatch("{id:int?}")]
    [ServiceFilter(typeof(TransactionActionFilter))]
    public async Task<IApiResult> PatchAsync(int? id, [FromBody] TrailUpdateDto trailUpdateDto)
    {
        if (!id.HasValue)
            return new ApiResult(false, ModelState);

        var existingModel = await _trailRepository.GetByIdAsync(id.Value);
        if (existingModel is null)
        {
            ModelState.AddModelError("ModelError", $"Trail {trailUpdateDto.Id} does not exist!");
            return new ApiResult(false, ModelState);
        }

        existingModel.Edit(trailUpdateDto.Name, trailUpdateDto.Distance, trailUpdateDto.Elevation, trailUpdateDto.Difficult);
        await _trailRepository.UpdateAsync(existingModel);

        return new ApiResult(success: true);
    }

    [HttpDelete("{id:int?}")]
    [ServiceFilter(typeof(TransactionActionFilter))]
    public async Task<IApiResult> DeleteAsync(int? id)
    {
        if (!id.HasValue)
            return new ApiResult(false, ModelState);

        var existingModel = await _trailRepository.GetByIdAsync(id.Value);
        if (existingModel is null)
        {
            ModelState.AddModelError("ModelError", $"Trail {id.Value} does not exist!");
            return new ApiResult(false, ModelState);
        }

        existingModel.Delete();
        await _trailRepository.UpdateAsync(existingModel);

        return new ApiResult(success: true);
    }
}
