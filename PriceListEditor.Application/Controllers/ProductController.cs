using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceListEditor.Application.Models;
using PriceListEditor.Application.ViewModels;

namespace PriceListEditor.Application.Controllers;

public class ProductController : Controller
{
    private IProductRepository productRepository;
    public int pageSize = 2;//number of product on page
    public ProductController(IProductRepository _productRepository)
    {
        productRepository = _productRepository;
    }
    //public async Task<IActionResult> ProductList(int page=1)
    public async Task<IActionResult> ProductList(int page = 1)
    {
        IQueryable<Product>? productSource = productRepository.Products; //source of products
        int produtCount = await productSource!.CountAsync(); //total number of products
        List<Product> pageProducts = await productSource!.Skip((page - 1)*pageSize).Take(pageSize).ToListAsync();//list of products on a page
        PageViewModel pageViewModel = new(page,pageSize, produtCount);
        ProductListViewModel productListViewModel = new() 
        {
            PageViewModel = pageViewModel,
            Products = pageProducts
        };
        return View(productListViewModel);
    }
}
