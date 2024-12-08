using System.ComponentModel.DataAnnotations;

namespace EMS.Core.DTOs;

public class AddUpdateDesignationDto
{
    [Required(ErrorMessage = "Designation name is required.")]
    [StringLength(100, ErrorMessage = "Designation name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
}
