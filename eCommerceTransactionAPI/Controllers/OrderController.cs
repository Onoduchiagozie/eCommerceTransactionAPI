 using eCommerceTransactionAPI.Application.Interface;
 using Microsoft.AspNetCore.Mvc;
 using WebApplication1.Service.DTOs;

 namespace eCommerceTransactionAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(
        [FromBody] PlaceOrderRequest request)
    {
        try
        {
            var orderId = await _service.PlaceOrderAsync(request);

            return Ok(new
            {
                message = "Order placed successfully",
                orderId
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}