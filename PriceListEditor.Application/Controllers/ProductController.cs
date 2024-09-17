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
    public ViewResult ProductList(string? category, int page = 1)
    {
        IQueryable<Product>? productSource = productRepository.Products; //source of products
        int produtCount = productSource!.Count(); //total number of products
        IQueryable<Category>? categorySource = productRepository.Categories;
        Category? selectedCategory = categorySource!.Where(c => category == null || c.CategoryName == category).FirstOrDefault() ?? new Category();
        List<Product> pageProducts = productSource!.Where(p => category == null || p.CategoryID == selectedCategory.CategoryID).Skip((page - 1) * pageSize).Take(pageSize).ToList();//list of products on a page
        PageViewModel pageViewModel = new(page, pageSize, produtCount);
        ProductListViewModel productListViewModel = new()
        {
            PageViewModel = pageViewModel,
            Products = pageProducts,
            CurrentCategoryId = selectedCategory.CategoryID,
            CurrentCategory = selectedCategory.CategoryName
        };
        return View(productListViewModel);
    }
}
