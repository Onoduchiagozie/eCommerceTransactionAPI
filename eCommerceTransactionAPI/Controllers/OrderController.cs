using Microsoft.AspNetCore.Mvc;

namespace eCommerceTransactionAPI.Controllers;

public class OrderController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}