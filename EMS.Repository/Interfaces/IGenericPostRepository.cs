namespace EMS.Repository.Interfaces;

public interface IGenericPostRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
