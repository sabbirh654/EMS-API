namespace EMS.Core.DTOs;

public class UpdateAttendanceDto
{
    public TimeSpan CheckInTime { get; set; }
    public TimeSpan CheckOutTime { get; set; }
}
