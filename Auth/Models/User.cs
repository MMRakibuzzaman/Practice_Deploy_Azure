using System.ComponentModel.DataAnnotations;

namespace Auth.Models;

public class User
{
    public int UserId { get; set; }
    [Required, MaxLength(250)] public string? Email { get; set; }
    [Required, MaxLength(255)] public string? PasswordHash { get; set; }
}