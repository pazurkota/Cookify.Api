using Cookify.Api.Dtos;

namespace Cookify.Api.Abstractions;

public record AuthResult(bool Succeeded, string? Token = null, IEnumerable<string>? Errors = null);

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterUserDto dto);
    Task<AuthResult> LoginAsync(LoginUserDto dto);
}
