



using eCommerceTransactionAPI.Application.Interface;
using eCommerceTransactionAPI.DAL.Data;
using eCommerceTransactionAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceTransactionAPI.DAL.Services;
 

public class ProductService : IProductService
{
    private readonly CommerceDbContext _context;

    public ProductService(CommerceDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(int id, Product updated)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing == null)
            throw new Exception("Product not found");

        if (updated.Name != null) existing.Name = updated.Name;
        if (updated.Description != null) existing.Description = updated.Description;
        if (updated.Quantity.HasValue) existing.Quantity = updated.Quantity.Value;
        if (updated.Price.HasValue) existing.Price = updated.Price.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new Exception("Product not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
