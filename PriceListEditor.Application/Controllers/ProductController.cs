using Microsoft.AspNetCore.Mvc;
using PriceListEditor.Application.Models;

namespace PriceListEditor.Application.Controllers;

public class ProductController : Controller
{
    private IProductRepository productRepository;
    public ProductController(IProductRepository _productRepository)
    {
        productRepository = _productRepository;
    }
    public IActionResult ListOfProducts()
    {
        return View(productRepository.Products);
    }
}
