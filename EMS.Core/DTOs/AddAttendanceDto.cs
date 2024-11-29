namespace EMS.Core.DTOs;

public class AddAttendanceDto
{
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan CheckInTime { get; set; }
    public TimeSpan CheckOutTime { get; set; }
}
