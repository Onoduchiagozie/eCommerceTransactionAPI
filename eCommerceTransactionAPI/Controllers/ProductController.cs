using eCommerceTransactionAPI.DAL.Data;
using eCommerceTransactionAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace eCommerceTransactionAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly CommerceDbContext _context;

    public ProductController(CommerceDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetProducts()
    {
        var allProducts = await _context.Products.ToListAsync();
        return Ok(allProducts);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return Ok(product);
    }

 
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

 
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product updated)
    {

        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null) return NotFound();

        if (updated.Name != null) existingProduct.Name = updated.Name;
        if (updated.Description != null) existingProduct.Description = updated.Description;
        if (updated.Quantity.HasValue) existingProduct.Quantity = updated.Quantity.Value;
        if (updated.Price.HasValue) existingProduct.Price = updated.Price.Value;


        try
        {
            // 4. Save changes
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(409, "Concurrency conflict occurred. Try again.");
        }

        // 5. Return 204 No Content (standard for PUT)
        return NoContent();
    }
 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}