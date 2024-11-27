using EMS.Core.Entities;
using EMS.Repository.Interfaces;

namespace EMS.Repository.Implementations;

public class AttendanceRepository : IAttendanceRepository
{
    public Task<IEnumerable<Attendance>?> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Attendance?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
