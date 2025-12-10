using eCommerceTransactionAPI.Domain.Models;

namespace eCommerceTransactionAPI.Application.Interface;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(int id, Product updated);
    Task DeleteAsync(int id);
}
