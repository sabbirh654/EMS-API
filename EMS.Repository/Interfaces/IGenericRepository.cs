using EMS.Core.Models;

namespace EMS.Repository.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<ApiResult> GetAllAsync();
    Task<ApiResult> GetByIdAsync(int id);
    Task<ApiResult> AddAsync(T entity);
    Task<ApiResult> UpdateAsync(T entity);
    Task<ApiResult> DeleteAsync(int id);
}
