using EMS.Core.DTOs;
using EMS.Core.Entities;

namespace EMS.Core.Mappers;

public static class AttendanceMapper
{
    public static Attendance MapAttendanceAddDto(this AddAttendanceDto dto)
    {
        return new Attendance
        {
            EmployeeId = dto.EmployeeId,
            Date = dto.Date,
            CheckInTime = dto.CheckInTime,
            CheckOutTime = dto.CheckOutTime,
        };
    }

    public static Attendance MapAttendanceUpdateDto(this UpdateAttendanceDto dto)
    {
        return new Attendance
        {
            CheckInTime = dto.CheckInTime,
            CheckOutTime = dto.CheckOutTime,
        };
    }
}
