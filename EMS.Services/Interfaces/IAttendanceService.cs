using EMS.Core.DTOs;
using EMS.Core.Entities;

namespace EMS.Services.Interfaces;

public interface IAttendanceService
{
    Task<IEnumerable<Attendance>?> GetAllAttendance(AttendanceFilter filter);
    Task<IEnumerable<AttendanceDetails>?> GetEmployeeAttendance(int employeeId);
    Task<Attendance?> GetAttendanceById(int id);
    Task AddAttendance(AddAttendanceDto dto);
    Task UpdateAttendance(int id, UpdateAttendanceDto dto);
    Task DeleteAttendance(int id);
}
