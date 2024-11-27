namespace EMS.Core.DTOs;

public class AddEmployeeDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime BirthDate { get; set; }
    public int DepartmentId { get; set; }
    public int DesignationId { get; set; }
}
