using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Filters;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Models.Interfaces;
using ParkyApi.Repositories.Interfaces;

namespace ParkyApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UsersController(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    [HttpPost]
    [ServiceFilter(typeof(TransactionActionFilter))]
    public async Task<IApiResult> PostAsync([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return new ApiResult(false, ModelState);

        var userExists = await _userRepository.UserExistsAsync(x => x.Username == userDto.Username);
        if (userExists)
        {
            ModelState.AddModelError("ModelError", "User already exists!");
            return new ApiResult(false, ModelState);
        }

        var model = _mapper.Map<User>(userDto);
        model.HasCreated();
        var modelCreated = await _userRepository.CreateAsync(model);

        var resultDto = _mapper.Map<UserDto>(modelCreated);

        return new ApiResult(resultDto);
    }

    [HttpPost("Authenticate")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userAuthenticated = await _userRepository.AuthenticateAsync(userLoginDto.Username, userLoginDto.Password);
        if (userAuthenticated is null)
        {
            ModelState.AddModelError("ModelError", "Username or password is incorrect!");
            return BadRequest(ModelState);
        }

        var resultDto = _mapper.Map<UserDto>(userAuthenticated);

        return Ok(resultDto);
    }
}
