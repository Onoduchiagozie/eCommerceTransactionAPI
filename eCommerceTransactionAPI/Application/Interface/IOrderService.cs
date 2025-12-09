using WebApplication1.Service.DTOs;

namespace eCommerceTransactionAPI.Application.Interface;

public interface IOrderService
{
    Task<int> PlaceOrderAsync(PlaceOrderRequest request);

}