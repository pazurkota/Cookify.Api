using Cookify.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Api.Database.Repositories;

public class RecipeRepository(IAppDbContext context) : IRecipeRepository
{
    public async Task<IEnumerable<Recipe>> GetAllAsync()
        => await context.Recipes.ToListAsync();

    public async Task<Recipe?> GetByIdAsync(int id)
        => await context.Recipes.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<Recipe> CreateAsync(Recipe recipe)
    {
        context.Recipes.Add(recipe);
        await context.SaveChangesAsync();
        return recipe;
    }

    public async Task<Recipe?> UpdateAsync(int id, Recipe recipe)
    {
        var existing = await context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
        if (existing is null) return null;

        existing.Title = recipe.Title;
        existing.Content = recipe.Content;

        context.Recipes.Update(existing);
        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
        if (existing is null) return false;

        context.Recipes.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
