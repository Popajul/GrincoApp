using grincoMessageApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace grincoMessageApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _userRepository;

    public UsersController(ILogger<UsersController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAll()
    {
        var users =  await _userRepository.GetAllUsers();
        return Ok(users);
    }
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUserById([FromQuery] int id)
    {
        var user =  await _userRepository.GetUserById(id);
        return Ok(user);
    }[HttpGet]
    [Route("by_login/{login}")]
    public async Task<IActionResult> GetUserByLogin([FromRoute] string login)
    {
        var user =  await _userRepository.GetUserByLogin(login);
        return Ok(user);
    }

}
