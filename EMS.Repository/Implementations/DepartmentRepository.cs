using EMS.Core.Entities;
using EMS.Repository.Interfaces;

namespace EMS.Repository.Implementations;

public class DepartmentRepository : IDepartmentRepository
{
    public Task<int> AddAsync(Department entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Department>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Department> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Department entity)
    {
        throw new NotImplementedException();
    }
}
