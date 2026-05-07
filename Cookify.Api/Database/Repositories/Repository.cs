using Microsoft.EntityFrameworkCore;

namespace Cookify.Api.Database.Repositories;

public abstract class Repository<T, TKey>(IAppDbContext context) : IRepository<T, TKey>
    where T : class
{
    protected readonly IAppDbContext Context = context;

    public async Task<IEnumerable<T>> GetAllAsync()
        => await Context.Set<T>().ToListAsync();

    public async Task<T?> GetByIdAsync(TKey id)
        => await Context.Set<T>().FindAsync(id);

    public async Task<T> CreateAsync(T entity)
    {
        Context.Set<T>().Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public abstract Task<T?> UpdateAsync(TKey id, T entity);

    public async Task<bool> DeleteAsync(TKey id)
    {
        var entity = await Context.Set<T>().FindAsync(id);
        if (entity is null) return false;

        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }
}
