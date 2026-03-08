using Cookify.Api.Abstractions;
using Cookify.Api.Dtos;
using Cookify.Api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cookify.Api.Controllers;

public class AuthController
    (UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService) : BaseController
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

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.UserLogin);
        if (user == null) await userManager.FindByNameAsync(dto.UserLogin);
        
        // if user wasn't found by email or login, throw 403
        if (user == null) return Unauthorized("Invalid login or password");

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

        if (result.Succeeded)
        {
            var roles = await userManager.GetRolesAsync(user);
            var token = tokenService.GenerateJwtToken(user, roles);
            
            return Ok(new { AccessToken = token });
        }
        
        return Unauthorized("Invalid login or password");
    }
}