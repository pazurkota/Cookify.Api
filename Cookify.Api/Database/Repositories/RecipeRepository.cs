using Cookify.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Api.Database.Repositories;

public class RecipeRepository(IAppDbContext context) : Repository<Recipe, int>(context)
{
    public override async Task<IEnumerable<Recipe>> GetAllAsync()
        => await Context.Recipes.Include(r => r.Author).ToListAsync();

    public override async Task<Recipe?> GetByIdAsync(int id)
        => await Context.Recipes.Include(r => r.Author).FirstOrDefaultAsync(r => r.Id == id);

    public override async Task<Recipe> CreateAsync(Recipe entity)
    {
        entity.CreatedAt = DateTime.UtcNow;

        Context.Recipes.Add(entity);
        await Context.SaveChangesAsync();

        return await Context.Recipes
            .Include(r => r.Author)
            .FirstAsync(r => r.Id == entity.Id);
    }

    public override async Task<Recipe?> UpdateAsync(int id, Recipe entity)
    {
        var existing = await Context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
        if (existing is null) return null;

        existing.Title = entity.Title;
        existing.Content = entity.Content;

        Context.Recipes.Update(existing);
        await Context.SaveChangesAsync();
        return existing;
    }
}
