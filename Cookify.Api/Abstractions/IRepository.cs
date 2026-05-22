namespace Cookify.Api.Database.Repositories;

public interface IRepository<T, TKey> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(TKey id);
    Task<T> CreateAsync(T entity);
    Task<T?> UpdateAsync(TKey id, T entity);
    Task<bool> DeleteAsync(TKey id);
}
