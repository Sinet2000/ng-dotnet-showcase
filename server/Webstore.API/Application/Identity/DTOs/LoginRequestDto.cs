using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Webstore.API.Application.Identity.DTOs;

public record LoginRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [JsonPropertyName("password")]
    public required string Password { get; init; }
}
