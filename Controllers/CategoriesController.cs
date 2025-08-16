using Microsoft.AspNetCore.Mvc;
using capacitaciones_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController(CapacitacionesPruebasContext context) : ControllerBase
{
    readonly CapacitacionesPruebasContext _context = context;

    [HttpPost(Name = "categories/")]
    public async Task<ActionResult> CreateCategory(Categoria category)
    {
        if (category.Nombre is null || category.Nombre.Trim().Equals(""))
            return BadRequest();

        await _context.Categorias.AddAsync(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CategoryById), new { categoryId = category.IdCategoria }, category);
    }

    [HttpGet(Name = "categories/")]
    public async Task<ActionResult<List<Categoria>>> Categories()
        => new(await _context.Categorias.ToListAsync());

    [HttpGet("{categoryId}", Name = "/{categoryId}")]
    public async Task<ActionResult<Categoria>> CategoryById(int categoryId)
    {
        Categoria? category = await _context.Categorias.FindAsync(categoryId);

        return category is null ? NotFound() : category;
    }

    [HttpPut("{categoryId}", Name = "categories/{categoryId}")]
    public async Task<ActionResult> UpdateCategory(int categoryId, Categoria category)
    {
        Categoria? storedCategory = await _context.Categorias.FindAsync(categoryId);

        if (storedCategory is null)
            return NotFound();

        if (category.Nombre is null || category.Nombre.Trim().Equals(""))
            return BadRequest();

        storedCategory.Nombre = category.Nombre;

        _context.Categorias.Update(storedCategory);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CategoryById), new { categoryId = storedCategory.IdCategoria }, storedCategory);

    }

    [HttpDelete("{categoryId}", Name = "categories/{categoryId}")]
    public async Task<ActionResult> DeleteCategory(int categoryId)
    {
        Categoria? category = await _context.Categorias.FindAsync(categoryId);

        if (category is null)
            return NotFound();

        _context.Categorias.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}