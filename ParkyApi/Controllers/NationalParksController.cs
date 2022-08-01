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
[Authorize]
public class NationalParksController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly INationalParkRepository _nationalParkRepository;

    public NationalParksController(IMapper mapper, INationalParkRepository nationalParkRepository)
    {
        _mapper = mapper;
        _nationalParkRepository = nationalParkRepository;
    }

    /// <summary>
    /// Get list of the national parks
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync()
    {
        var model = await _nationalParkRepository.GetAllAsync();

        var result = _mapper.Map<List<NationalParkDto>>(model);

        return new JsonResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var model = await _nationalParkRepository.GetByIdAsync(id);
        if (model is null) return NotFound();

        var result = _mapper.Map<NationalParkDto>(model);

        return new JsonResult(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(TransactionActionFilter))]
    public async Task<IApiResult> PostAsync([FromBody] NationalParkDto nationalParkDto)
    {
        if (!ModelState.IsValid)
            return new ApiResult(false, ModelState);

        var modelsExists = await _nationalParkRepository.NationalParkExistsAsync(x => x.Name == nationalParkDto.Name);
        if (modelsExists)
        {
            ModelState.AddModelError("ModelError", $"National Park {nationalParkDto.Id} does not exist!");
            return new ApiResult(false, ModelState);
        }

        var model = _mapper.Map<NationalPark>(nationalParkDto);
        model.HasCreated();
        var result = await _nationalParkRepository.CreateAsync(model);

        var resultDto = _mapper.Map<NationalParkDto>(result);

        return new ApiResult(resultDto);
    }

    [HttpPatch("{id:int?}")]
    [ServiceFilter(typeof(TransactionActionFilter))]
    public async Task<IApiResult> PatchAsync(int? id, [FromBody] NationalParkDto nationalParkDto)
    {
        if (!id.HasValue)
            return new ApiResult(false, ModelState);

        var existingModel = await _nationalParkRepository.GetByIdAsync(id.Value);
        if (existingModel is null)
        {
            ModelState.AddModelError("ModelError", $"National Park {nationalParkDto.Id} does not exist!");
            return new ApiResult(false, ModelState);
        }

        existingModel.DefinePicture(nationalParkDto.Picture);
        existingModel.Edit(nationalParkDto.Name,
                           nationalParkDto.State,
                           nationalParkDto.Established);

        await _nationalParkRepository.UpdateAsync(existingModel);

        return new ApiResult(success: true);
    }

    [HttpDelete("{id:int?}")]
    [ServiceFilter(typeof(TransactionActionFilter))]
    public async Task<IApiResult> DeleteAsync(int? id)
    {
        if (!id.HasValue)
            return new ApiResult(false, ModelState);

        var existingModel = await _nationalParkRepository.GetByIdAsync(id.Value);
        if (existingModel is null)
        {
            ModelState.AddModelError("ModelError", $"National Park {id.Value} does not exist!");
            return new ApiResult(false, ModelState);
        }

        existingModel.Delete();
        await _nationalParkRepository.UpdateAsync(existingModel);

        return new ApiResult(success: true);
    }
}
