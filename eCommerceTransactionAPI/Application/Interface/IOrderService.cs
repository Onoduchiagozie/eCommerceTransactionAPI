using eCommerceTransactionAPI.Domain.Models;
using WebApplication1.Service.DTOs;

namespace eCommerceTransactionAPI.Application.Interface;

public interface IOrderService
{
    Task<PlaceOrderResult> PlaceOrderAsync(PlaceOrderRequest request);

}