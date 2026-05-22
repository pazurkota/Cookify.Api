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

    /// <summary>
    /// Update an existing recipe
    /// </summary>
    /// <param name="id">Recipe id</param>
    /// <param name="dto">Recipe title and content</param>
    /// <returns>The updated recipe</returns>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditRecipe(int id, [FromBody] EditRecipeDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();

        var existing = await recipeRepository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        if (existing.AuthorId != userId) return Forbid();

        existing.Title = dto.Title;
        existing.Content = dto.Content;

        var updated = await recipeRepository.UpdateAsync(id, existing);
        if (updated is null) return Forbid();

        return Ok(updated);
    }

    /// <summary>
    /// Delete a recipe
    /// </summary>
    /// <param name="id">Recipe id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();

        var existing = await recipeRepository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        if (existing.AuthorId != userId) return Forbid();

        var deleted = await recipeRepository.DeleteAsync(id);
        if (!deleted) return Forbid();

        return NoContent();
    }
}
