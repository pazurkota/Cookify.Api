using System.ComponentModel.DataAnnotations;

namespace Cookify.Api.Dtos;

public record RegisterUserDto(
    [Required] string Username,
    [Required] string Email,
    [Required] [Range(8, 32)] string Password);