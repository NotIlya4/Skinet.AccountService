using Api.UserController.Views;
using Domain.Entities;
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
    private readonly EnumParser _enumParser;

    public UsersController(IUserService userService, ViewMapper mapper, EnumParser enumParser)
    {
        _userService = userService;
        _mapper = mapper;
        _enumParser = enumParser;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<JwtTokenPairView>> Register(RegisterCredentialsView registerCredentialsView)
    {
        JwtTokenPair jwtTokenPair = await _userService.Register(registerCredentialsView.Email, registerCredentialsView.Password);
        JwtTokenPairView view = _mapper.MapJwtTokenPair(jwtTokenPair);
        return Ok(view);
    }

    [HttpGet]
    [Route("login")]
    public async Task<ActionResult<JwtTokenPairView>> Login([FromQuery] RegisterCredentialsView registerCredentialsView)
    {
        JwtTokenPair jwtTokenPair = await _userService.Login(registerCredentialsView.Email, registerCredentialsView.Password);
        JwtTokenPairView view = _mapper.MapJwtTokenPair(jwtTokenPair);
        return Ok(view);
    }

    [HttpGet]
    [Route("logout")]
    public async Task<ActionResult> Logout(string userId, string refreshToken)
    {
        await _userService.Logout(new Guid(userId), new Guid(refreshToken));
        return NoContent();
    }
    
    [HttpGet]
    [Route("logoutFromAllDevices")]
    public async Task<ActionResult> LogoutFromAllDevices(string userId)
    {
        await _userService.LogOutInAllEntries(new Guid(userId));
        return NoContent();
    }
    
    [HttpGet]
    [Route("updateJwtPair")]
    public async Task<ActionResult<JwtTokenPairView>> UpdateJwtPair(string userId, string refreshToken)
    {
        JwtTokenPair jwtTokenPair = await _userService.UpdateJwtPair(new Guid(userId), new Guid(refreshToken));
        JwtTokenPairView view = _mapper.MapJwtTokenPair(jwtTokenPair);
        return Ok(view);
    }

    [HttpGet]
    [Route("validateJwtToken")]
    public ActionResult ValidateJwtToken(string jwtToken)
    {
        _userService.ValidateJwtToken(jwtToken);
        return NoContent();
    }

    [HttpGet]
    [Route("{property}/{value}")]
    public async Task<ActionResult<UserView>> GetUser(string property, string value)
    {
        User address = await _userService.GetUser(_enumParser.Parse<UserStrictFilterProperty>(property), value);
        UserView addressView = _mapper.MapUser(address);
        return Ok(addressView);
    }
}