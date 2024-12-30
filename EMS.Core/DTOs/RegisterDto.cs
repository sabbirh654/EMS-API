﻿using System.ComponentModel.DataAnnotations;

namespace EMS.Core.DTOs;

public class RegisterDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
