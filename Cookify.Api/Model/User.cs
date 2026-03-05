using Microsoft.AspNetCore.Identity;

namespace Cookify.Api.Model;

public class User : IdentityUser
{
    public required string Username { get; set; }
    public string? Bio { get; set; }
    public string? UserAvatarUrl { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.Now;
}