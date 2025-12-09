 
using Microsoft.EntityFrameworkCore;
using System.Data;
using eCommerceTransactionAPI.Application.Interface;
using eCommerceTransactionAPI.DAL.Data;
using eCommerceTransactionAPI.Domain.Models;
using Microsoft.Data.SqlClient;
using WebApplication1.Service.DTOs;

namespace DAL.Services;

public class OrderService : IOrderService
{
    private readonly CommerceDbContext _context;

    public OrderService(CommerceDbContext context)
    {
        _context = context;
    }

    public async Task<int> PlaceOrderAsync(PlaceOrderRequest request)
    {
        using var transaction = await _context.Database
            .BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            var order = new Order();

            foreach (var item in request.Items)
            {
                var product = await _context.Products
                    .FromSqlRaw(
                        @"SELECT * FROM Products 
                          WITH (UPDLOCK, ROWLOCK)
                          WHERE Id = @id",
                        new SqlParameter("@id", item.ProductId))
                    .FirstOrDefaultAsync();

                if (product == null)
                    throw new Exception("Product not found");

                if (product.Quantity < item.Quantity)
                    throw new Exception("Insufficient stock");

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

            return order.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}