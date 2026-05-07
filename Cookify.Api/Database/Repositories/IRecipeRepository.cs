using Cookify.Api.Model;

namespace Cookify.Api.Database.Repositories;

public interface IRecipeRepository
{
    Task<IEnumerable<Recipe>> GetAllAsync();
    Task<Recipe?> GetByIdAsync(int id);
    Task<Recipe> CreateAsync(Recipe recipe);
    Task<Recipe?> UpdateAsync(int id, Recipe recipe);
    Task<bool> DeleteAsync(int id);
}
