namespace EMS.Repository.Interfaces;

public interface IGenericPostRepository<T> where T : class
{
    Task<int> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}
