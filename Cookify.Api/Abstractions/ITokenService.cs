using Cookify.Api.Model;

namespace Cookify.Api.Abstractions;

public interface ITokenService
{
    string GenerateJwtToken(User user, IList<string> roles);
}