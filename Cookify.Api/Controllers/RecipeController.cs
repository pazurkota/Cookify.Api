using System.Security.Claims;
using Cookify.Api.Abstractions;
using Cookify.Api.Dtos;
using Cookify.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookify.Api.Controllers;

public class RecipeController(IRecipeRepository recipeRepository) : BaseController
{
    /// <summary>
    /// Get all recipes (paginated)
    /// </summary>
    /// <param name="query">Page number and page size (max 100)</param>
    /// <returns>Paged list of recipes</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRecipes([FromQuery] RecipePageQuery query)
    {
        var result = await recipeRepository.GetPagedAsync(query);
        return Ok(result);
    }

    /// <summary>
    /// Get a recipe by id
    /// </summary>
    /// <param name="id">Recipe id</param>
    /// <returns>The recipe</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecipeById(int id)
    {
        var recipe = await recipeRepository.GetByIdAsync(id);
        if (recipe is null) return NotFound();
        return Ok(recipe);
    }

    /// <summary>
    /// Add a new recipe
    /// </summary>
    /// <param name="dto">Recipe title and content</param>
    /// <returns>The created recipe</returns>
    [HttpPost("create")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();

        var recipe = new Recipe
        {
            Title = dto.Title,
            Content = dto.Content,
            AuthorId = userId
        };

        var created = await recipeRepository.CreateAsync(recipe);
        return CreatedAtAction(nameof(CreateRecipe), new { id = created.Id }, created);
    }
}
