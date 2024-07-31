using Microsoft.AspNetCore.Mvc;

namespace PriceListEditor.Application.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() =>View();
}
