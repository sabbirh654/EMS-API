using EMS.Core.Entities;
using EMS.Repository.Interfaces;

namespace EMS.Repository.Implementations;

public class DepartmentRepository : IDepartmentRepository
{
    public Task AddAsync(Department entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Department>?> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Department?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Department entity)
    {
        throw new NotImplementedException();
    }
}
