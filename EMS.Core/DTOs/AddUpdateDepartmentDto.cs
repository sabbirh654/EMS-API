using System.ComponentModel.DataAnnotations;

namespace EMS.Core.DTOs;

public class AddUpdateDepartmentDto
{
    [Required(ErrorMessage = "Department name is required.")]
    [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
}
