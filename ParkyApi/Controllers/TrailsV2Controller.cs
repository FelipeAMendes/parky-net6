using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models.Dtos;
using ParkyApi.Repositories.Interfaces;

namespace ParkyApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
public class TrailsV2Controller : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITrailRepository _trailRepository;

    public TrailsV2Controller(IMapper mapper, ITrailRepository trailRepository)
    {
        _mapper = mapper;
        _trailRepository = trailRepository;
    }

    /// <summary>
    /// (V2) Get list of trails
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var model = await _trailRepository.GetAllAsync();

        var result = _mapper.Map<List<TrailV2Dto>>(model);
        
        return new JsonResult(result);
    }
}
