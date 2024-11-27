namespace EMS.Core.DTOs;

public class UpdateEmployeeDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime BirthDate { get; set; }
}