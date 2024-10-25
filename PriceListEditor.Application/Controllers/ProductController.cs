using Microsoft.AspNetCore.Mvc;
using PriceListEditor.Application.Models;
using PriceListEditor.Application.ViewModels;

namespace PriceListEditor.Application.Controllers;

public class ProductController : Controller
{
    private IProductRepository productRepository;
    public int PageSize = 2;
    public ProductController(IProductRepository _productRepository)
    {
        productRepository = _productRepository;
    }
    public ViewResult ProductList(string? category, int productPage = 1)
    {
        Category? CurrentCategory = category == null ? null : productRepository.Categories.Where(e => e.CategoryName == category).FirstOrDefault();
        return View(new ProductsListViewModel
        {
            Products = productRepository
            .Products
            .Where(p => CurrentCategory == null || p.CategoryID == CurrentCategory.CategoryID)
            .OrderBy(p => p.ProductID).Skip((productPage - 1) * PageSize).Take(PageSize),
            PageViewModel = new PageViewModel
            {
                CurrenPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = category == null ?
                productRepository.Products.Count()
                : productRepository.Products.Where(e => e.CategoryID == CurrentCategory!.CategoryID).Count()
            },
            CurrentCategory = (CurrentCategory ?? new Category { CategoryName = null ?? "" }).CategoryName
        });
    }
}
