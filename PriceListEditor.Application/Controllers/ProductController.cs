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
        long? CurrentCategoryID = category == null ? null : (productRepository.Categories.Where(e => e.CategoryName == category).FirstOrDefault() ?? new Category { CategoryID = null }).CategoryID;
        return View(new ProductsListViewModel
        {
            Products = productRepository
            .Products
            .Where(p => CurrentCategoryID == null || p.CategoryID == CurrentCategoryID)
            .OrderBy(p => p.ProductID).Skip((productPage - 1) * PageSize).Take(PageSize),
            PagingInfo = new PagingInfo
            {
                CurrenPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = productRepository.Products.Count()
            },
            CurrentCategory = CurrentCategoryID
        });
    }

    //public ViewResult ProductList(long? category, int productPage = 1)
    //{
    //    return View(new ProductsListViewModel
    //    {
    //        Products = productRepository
    //        .Products
    //        .Where(p => category == null || p.CategoryID == category)
    //        .OrderBy(p => p.ProductID).Skip((productPage - 1) * PageSize).Take(PageSize),
    //        PagingInfo = new PagingInfo
    //        {
    //            CurrenPage = productPage,
    //            ItemsPerPage = PageSize,
    //            TotalItems = productRepository.Products.Count()
    //        },
    //        CurrentCategory = category
    //    });
    //}


    ////public ViewResult ProductList(long? category, int productPage = 1)
    //public ViewResult ProductList(string? category, int productPage = 1)
    //{
    //    Category? CurrentCategory = category == null ? null : productRepository.Categories.Where(e => e.CategoryName == category) as Category;
    //    //((productRepository.Categories).Where(e => e.CategoryID == category) as Category); 
    //    return View(new ProductsListViewModel
    //    {
    //        Products = productRepository
    //        .Products
    //        .Where(p => CurrentCategory == null || p.CategoryID == CurrentCategory.CategoryID)
    //        .OrderBy(p => p.ProductID).Skip((productPage - 1) * PageSize).Take(PageSize),
    //        PagingInfo = new PagingInfo
    //        {
    //            CurrenPage = productPage,
    //            ItemsPerPage = PageSize,
    //            TotalItems = productRepository.Products.Count()
    //        },
    //        CurrentCategory = CurrentCategory
    //    });
    //    //return View(new ProductsListViewModel
    //    //{
    //    //    Products = productRepository
    //    //    .Products
    //    //    .Where(p=> category == null || p.CategoryID == category)
    //    //    .OrderBy(p => p.ProductID).Skip((productPage - 1) * PageSize).Take(PageSize),
    //    //    PagingInfo = new PagingInfo 
    //    //    {
    //    //        CurrenPage = productPage,
    //    //        ItemsPerPage = PageSize,
    //    //        TotalItems = productRepository.Products.Count()
    //    //    },
    //    //    CurrentCategory = category
    //    //});
    //}
}
