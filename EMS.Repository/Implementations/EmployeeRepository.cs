using EMS.Core.Entities;
using EMS.Repository.Interfaces;

namespace EMS.Repository.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    public Task<int> AddAsync(Employee entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<EmployeeDetails>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeDetails> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Employee entity)
    {
        throw new NotImplementedException();
    }
}
