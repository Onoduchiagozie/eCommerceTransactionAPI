namespace eCommerceTransactionAPI.Domain.Models;

public class PlaceOrderResult
{
    public int? OrderId { get; set; }

    public List<FulfilledItem> Fulfilled { get; set; } = new();
    public List<FailedItem> Failed { get; set; } = new();
}

public class FailedItem
{
    public int ProductId { get; set; }
    public string Reason { get; set; }
    public int? Available { get; set; }
    public int? Requested { get; set; }
}


public class FulfilledItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}

