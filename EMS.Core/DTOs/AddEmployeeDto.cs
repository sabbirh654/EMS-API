using System.ComponentModel.DataAnnotations;

namespace EMS.Core.DTOs;

public class AddEmployeeDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string? PhoneNumber { get; set; }

    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "BirthDate is required.")]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "Department ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Department ID must be greater than zero.")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Designation ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Designation ID must be greater than zero.")]
    public int DesignationId { get; set; }
}
