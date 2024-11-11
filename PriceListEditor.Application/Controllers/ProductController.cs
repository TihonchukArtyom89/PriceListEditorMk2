using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using PriceListEditor.Application.Models;
using PriceListEditor.Application.ViewModels;

namespace PriceListEditor.Application.Controllers;

public class ProductController : Controller
{
    private IProductRepository productRepository;
    public ProductController(IProductRepository _productRepository)
    {
        productRepository = _productRepository;
    }
    public ViewResult ProductList(string? category, int productPage = 1, int pageSize = 1)
    {
        Category? CurrentCategory = category == null ? null : productRepository.Categories.Where(e => e.CategoryName == category).FirstOrDefault();
        IEnumerable<Product> products = productRepository.Products.Where(p => CurrentCategory == null || p.CategoryID == CurrentCategory.CategoryID).OrderBy(p => p.ProductID).Skip((productPage - 1) * pageSize).Take(pageSize);
        products = products.Count() != 0 ? products :
            products.Append(
                new Product()
                {
                    CategoryID = 0,
                    ProductID = 0,
                    ProductName = "Нет в наличии!",
                    ProductDescription = "Продуктов категории " + (CurrentCategory ?? new Category() { CategoryName = "Категория не указана" }).CategoryName + " не имеется!",
                    ProductPrice = 0.00M,
                });
        return View(new ProductsListViewModel
        {
            Products = products,
            PageViewModel = new PageViewModel
            {
                CurrenPage = productPage,
                PageSize = pageSize,
                TotalItems = category == null ?
                productRepository.Products.Count() : productRepository.Products.Where(e => e.CategoryID == CurrentCategory!.CategoryID).Count(),
                CurrentAction = "Products"
            },
            CurrentCategory = (CurrentCategory ?? new Category { CategoryName = null ?? "" }).CategoryName,

        });
    }
}
