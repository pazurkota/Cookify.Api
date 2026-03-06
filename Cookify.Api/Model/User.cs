using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Cookify.Api.Model;


public class User : IdentityUser
{
    public string Username { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? UserAvatarUrl { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.Now;
    public List<Recipe> Recipes { get; set; } = new();
}