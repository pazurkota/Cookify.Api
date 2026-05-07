using Cookify.Api.Abstractions;
using Cookify.Api.Dtos;
using Cookify.Api.Model;
using Microsoft.AspNetCore.Identity;

namespace Cookify.Api.Services;

public class AuthService(UserManager<User> userManager, ITokenService tokenService) : IAuthService
{
    public async Task<AuthResult> RegisterAsync(RegisterUserDto dto)
    {
        var user = new User
        {
            UserName = dto.Username,
            Email = dto.Email
        };

        var result = await userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
            return new AuthResult(true);

        var errors = result.Errors.Select(e => e.Description);
        return new AuthResult(false, Errors: errors);
    }

    public async Task<AuthResult> LoginAsync(LoginUserDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.UserLogin)
                   ?? await userManager.FindByNameAsync(dto.UserLogin);

        if (user is null)
            return new AuthResult(false);

        var passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValid)
            return new AuthResult(false);

        var roles = await userManager.GetRolesAsync(user);
        var token = tokenService.GenerateJwtToken(user, roles);
        return new AuthResult(true, Token: token);
    }
}
