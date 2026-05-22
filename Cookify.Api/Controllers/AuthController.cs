using Cookify.Api.Abstractions;
using Cookify.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Cookify.Api.Controllers;

public class AuthController(IAuthService authService) : BaseController
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="dto">The user credentials (username, email, password)</param>
    /// <returns></returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterNewUser([FromBody] RegisterUserDto dto)
    {
        var result = await authService.RegisterAsync(dto);

        if (result.Succeeded)
            return Ok(new { message = "Account created successfully" });

        return BadRequest(new { Errors = result.Errors });
    }

    /// <summary>
    /// Log the user
    /// </summary>
    /// <param name="dto">The user credentials (username/email and password)</param>
    /// <returns>JWT Bearer key</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
    {
        var result = await authService.LoginAsync(dto);

        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid login or password" });

        return Ok(new { AccessToken = result.Token });
    }
}
