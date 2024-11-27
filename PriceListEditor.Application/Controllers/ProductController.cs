using Microsoft.AspNetCore.Mvc;
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
    public ViewResult ProductList(string? category, SortOrder priceSortOrder = SortOrder.PriceDesc, int productPage = 1, int pageSize = 1)
    {
        ViewBag.SelectedPageSize = pageSize;
        ViewBag.SelectedCategory = category;
        ViewBag.PriceSortOrder = priceSortOrder;
        //ViewBag.PriceSortOrder = priceSortOrder == SortOrder.PriceDesc ? SortOrder.PriceAsc : SortOrder.PriceDesc;
        ViewBag.SortingText = priceSortOrder == SortOrder.PriceDesc ? "От дорогих к дешёвым" : "От дешёвых к дорогим";
        Category? CurrentCategory = category == null ? null : productRepository.Categories.Where(e => e.CategoryName == category).FirstOrDefault();
        IEnumerable<Product> products = productRepository.Products.Where(p => CurrentCategory == null || p.CategoryID == CurrentCategory.CategoryID).OrderBy(p => p.ProductID).Skip((productPage - 1) * pageSize).Take(pageSize);
        if(products.Count() == 0 && productPage != 1)
        {
            productPage = 1;
            products = productRepository.Products.Where(p => CurrentCategory == null || p.CategoryID == CurrentCategory.CategoryID).OrderBy(p => p.ProductID).Skip((productPage - 1) * pageSize).Take(pageSize);
        }
        ViewBag.ProductCount = products.Count();
        ViewBag.SelectedPage = productPage;
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
        products = priceSortOrder switch
        {
            SortOrder.PriceAsc => products.OrderBy(e => e.ProductPrice),
            SortOrder.PriceDesc => products.OrderByDescending(e => e.ProductPrice),
            _=>products.OrderBy(e=>e.ProductPrice)
        };
        ViewBag.PriceSortOrder = priceSortOrder == SortOrder.PriceDesc ? SortOrder.PriceAsc : SortOrder.PriceDesc;
        ProductsListViewModel viewModel = new ProductsListViewModel
        {
            Products = products,
            PageViewModel = new PageViewModel
            {
                CurrenPage = ViewBag.SelectedPage,
                PageSize = pageSize,
                TotalItems = category == null ? productRepository.Products.Count() : productRepository.Products.Where(e => e.CategoryID == CurrentCategory!.CategoryID).Count(),                
                Pseudonym = "Products"
            },
            CurrentCategory = (CurrentCategory ?? new Category { CategoryName = null ?? "" }).CategoryName,
        };
        return View(viewModel);
    }
}
