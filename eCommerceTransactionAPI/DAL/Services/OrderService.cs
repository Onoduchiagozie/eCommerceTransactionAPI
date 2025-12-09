 
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using eCommerceTransactionAPI.Application.Interface;
using eCommerceTransactionAPI.DAL.Data;
using eCommerceTransactionAPI.Domain.Models;
using WebApplication1.Service.DTOs;


namespace eCommerceTransactionAPI.DAL.Services;

public class OrderService : IOrderService
{
    private readonly CommerceDbContext _context;

    public OrderService(CommerceDbContext context)
    {
        _context = context;
    }

    public async Task<int> PlaceOrderAsync(PlaceOrderRequest request)
    {
        var start = DateTime.UtcNow;
        Console.WriteLine($"[{start:HH:mm:ss.fff}] Order started");

        using var transaction =
            await _context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable);

        try
        {
            var order = new Order();

            foreach (var item in request.Items)
            {
                // ✅ ROW + UPDATE LOCK
                var product = await _context.Products
                    .FromSqlRaw(
                        @"SELECT * FROM Products
                          WITH (UPDLOCK, ROWLOCK)
                          WHERE Id = @id",
                        new SqlParameter("@id", item.ProductId))
                    .FirstOrDefaultAsync();

                Console.WriteLine(
                    $"[{DateTime.UtcNow:HH:mm:ss.fff}] Product locked");

 

                if (product == null)
                    throw new Exception($"Product not found (ID: {item.ProductId})");

                if (product.Quantity < item.Quantity)
                    throw new Exception(
                        $"Insufficient stock for '{product.Name}'. " +
                        $"Available: {product.Quantity}, Requested: {item.Quantity}");

                product.Quantity -= item.Quantity;

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            Console.WriteLine(
                $"[{DateTime.UtcNow:HH:mm:ss.fff}] Order committed");

            return order.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    
}
