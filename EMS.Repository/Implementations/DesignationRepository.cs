using EMS.Core.Entities;
using EMS.Repository.Interfaces;

namespace EMS.Repository.Implementations;

public class DesignationRepository : IDesignationRepository
{
    public Task<int> AddAsync(Designation entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Designation>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Designation> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Designation entity)
    {
        throw new NotImplementedException();
    }
}
