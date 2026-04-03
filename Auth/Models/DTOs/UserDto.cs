namespace Auth.Models.DTOs;

public record RegisterDto(string Email, string Password);

public record LoginDto(string Email, string Password);