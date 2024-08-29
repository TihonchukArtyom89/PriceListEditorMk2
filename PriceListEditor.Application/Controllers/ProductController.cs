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
    public ViewResult ProductList(int page = 1)
    {
        IQueryable<Product>? productSource = productRepository.Products; //source of products
        int produtCount = productSource!.Count(); //total number of products
        List<Product> pageProducts =  productSource!.Skip((page - 1)*pageSize).Take(pageSize).ToList();//list of products on a page
        PageViewModel pageViewModel = new(page,pageSize, produtCount);
        ProductListViewModel productListViewModel = new() 
        {
            PageViewModel = pageViewModel,
            Products = pageProducts
        };
        return View(productListViewModel);
    }
}
