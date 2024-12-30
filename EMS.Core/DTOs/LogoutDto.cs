using System.ComponentModel.DataAnnotations;

namespace EMS.Core.DTOs;

public class LogoutDto
{
    [Required]
    public string? RefreshToken { get; set; }
}