using Cookify.Api.Dtos;
using Cookify.Api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cookify.Api.Controllers;

public class AuthController(UserManager<User> userManager, SignInManager<User> signInManager) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterNewUser([FromBody] RegisterUserDto dto)
    {
        var user = new User
        {
            UserName = dto.Username,
            Email = dto.Email
        };

        var result = await userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            return Ok(new { message = "Account created successfully" });   
        }

        var errors = result.Errors.Select(x => x.Description);
        return BadRequest(new { Errors = errors });
    }
}