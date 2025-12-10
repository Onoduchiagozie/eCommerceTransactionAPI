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

    public async Task<PlaceOrderResult> PlaceOrderAsync(PlaceOrderRequest request)
    {
        using var transaction = await _context.Database
            .BeginTransactionAsync(IsolationLevel.Serializable);

        var result = new PlaceOrderResult();
        var order = new Order();

        foreach (var item in request.Items)
        {
            var product = await _context.Products
                .FromSqlRaw(
                    @"SELECT * FROM Products WITH (UPDLOCK, ROWLOCK)
                  WHERE Id = @id",
                    new SqlParameter("@id", item.ProductId))
                .FirstOrDefaultAsync();

            if (product == null)
            {
                result.Failed.Add(new FailedItem
                {
                    ProductId = item.ProductId,
                    Reason = "Product not found"
                });
                continue;
            }

            if (product.Quantity < item.Quantity)
            {
                result.Failed.Add(new FailedItem
                {
                    ProductId = product.Id,
                    Reason = "Insufficient stock",
                    Available = product.Quantity,
                    Requested = item.Quantity
                });
                continue;
            }

            product.Quantity -= item.Quantity;

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity
            });

            result.Fulfilled.Add(new FulfilledItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity
            });
        }

        if (result.Fulfilled.Any())
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        else
        {
            await transaction.RollbackAsync();
        }

        return result;
    }

}
