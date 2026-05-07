using Cookify.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Api.Database;

public interface IAppDbContext
{
    DbSet<Recipe> Recipes { get; set; }
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
