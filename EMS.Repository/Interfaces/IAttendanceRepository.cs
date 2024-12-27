using EMS.Core.Entities;
using EMS.Core.Models;

namespace EMS.Repository.Interfaces;

public interface IAttendanceRepository : IGenericRepository<Attendance>
{
    Task<ApiResult> GetAllAsync(AttendanceFilter filter);
    Task<ApiResult> GetAllByIdAsync(int id);
}
