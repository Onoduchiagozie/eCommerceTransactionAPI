namespace WebApplication1.Service.DTOs
{
         public class PlaceOrderRequest
        {
            public List<OrderItemRequest> Items { get; set; } = new();
        }    
}
