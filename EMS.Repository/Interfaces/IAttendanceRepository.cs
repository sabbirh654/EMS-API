using EMS.Core.Entities;
using EMS.Core.Models;

namespace EMS.Repository.Interfaces;

public interface IAttendanceRepository : IGenericGetRepository<Attendance>, IGenericPostRepository<Attendance>
{
    Task<ApiResult> GetAllAsync(AttendanceFilter filter);
    Task<ApiResult> GetAllByIdAsync(int id);
}
