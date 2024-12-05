using EMS.Core.DTOs;
using EMS.Core.Entities;
using EMS.Core.Models;

namespace EMS.Services.Interfaces;

public interface IAttendanceService
{
    Task<ApiResult> GetAllAttendance(AttendanceFilter filter);
    Task<ApiResult> GetEmployeeAttendance(int employeeId);
    Task<ApiResult> GetAttendanceById(int id);
    Task<ApiResult> AddAttendance(AddAttendanceDto dto);
    Task<ApiResult> UpdateAttendance(int id, UpdateAttendanceDto dto);
    Task<ApiResult> DeleteAttendance(int id);
}
