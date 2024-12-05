using EMS.Core.Models;

namespace EMS.Repository.Interfaces;

public interface IGenericGetRepository<T> where T : class
{
    Task<ApiResult> GetAllAsync();
    Task<ApiResult> GetByIdAsync(int id);
}
