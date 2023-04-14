using Api.UserController.Views;
using Domain.Primitives;
using Infrastructure.JwtTokenService;
using Infrastructure.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Api.UserController;

[ApiController]
[Route("api/users/")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ViewMapper _mapper;

    public UsersController(IUserService userService, ViewMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<JwtTokenPairView>> Register(RegisterCredentialsView registerCredentialsView)
    {
        JwtTokenPair jwtTokenPair = await _userService.Register(registerCredentialsView.Email, registerCredentialsView.Password);
        return Ok(_mapper.MapJwtTokenPair(jwtTokenPair));
    }

    [HttpGet]
    [Route("login")]
    public async Task<ActionResult<JwtTokenPairView>> Login([FromQuery] RegisterCredentialsView registerCredentialsView)
    {
        JwtTokenPair jwtTokenPair = await _userService.Login(registerCredentialsView.Email, registerCredentialsView.Password);
        return Ok(_mapper.MapJwtTokenPair(jwtTokenPair));
    }

    [HttpGet]
    [Route("logout")]
    public async Task<ActionResult> Logout(string userId, string refreshToken)
    {
        await _userService.Logout(new UserId(userId), new RefreshToken(refreshToken));
        return NoContent();
    }
    
    [HttpGet]
    [Route("logoutFromAllDevices")]
    public async Task<ActionResult> LogoutFromAllDevices(string userId)
    {
        await _userService.LogOutInAllEntries(new UserId(userId));
        return NoContent();
    }
    
    [HttpGet]
    [Route("updateJwtPair")]
    public async Task<ActionResult<JwtTokenPairView>> UpdateJwtPair(string userId, string refreshToken)
    {
        JwtTokenPair jwtTokenPair = await _userService.UpdateJwtPair(new UserId(userId), new RefreshToken(refreshToken));
        return Ok(_mapper.MapJwtTokenPair(jwtTokenPair));
    }

    [HttpGet]
    [Route("validateJwtToken")]
    public ActionResult ValidateJwtToken(string jwtToken)
    {
        _userService.ValidateJwtToken(jwtToken);
        return NoContent();
    }
}