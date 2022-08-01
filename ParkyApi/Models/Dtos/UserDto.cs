using System.ComponentModel.DataAnnotations;

namespace ParkyApi.Models.Dtos;

public class UserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Role { get; set; }
    public string? Token { get; set; }
}