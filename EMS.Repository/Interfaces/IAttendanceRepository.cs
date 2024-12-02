using EMS.Core.Entities;

namespace EMS.Repository.Interfaces;

public interface IAttendanceRepository : IGenericGetRepository<Attendance>, IGenericPostRepository<Attendance>
{
    Task<IEnumerable<Attendance>?> GetAllAsync(AttendanceFilter filter);
    Task<IEnumerable<AttendanceDetails>?> GetAllByIdAsync(int id);
}
