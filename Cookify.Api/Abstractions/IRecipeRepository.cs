using Cookify.Api.Database.Repositories;
using Cookify.Api.Dtos;
using Cookify.Api.Model;

namespace Cookify.Api.Abstractions;

public interface IRecipeRepository : IRepository<Recipe, int>
{
    Task<PagedResult<Recipe>> GetPagedAsync(RecipePageQuery query);
}
