using EMS.Core.Models;

namespace EMS.Repository.Interfaces;

public interface IGenericPostRepository<T> where T : class
{
    Task<ApiResult> AddAsync(T entity);
    Task<ApiResult> UpdateAsync(T entity);
    Task<ApiResult> DeleteAsync(int id);
}
