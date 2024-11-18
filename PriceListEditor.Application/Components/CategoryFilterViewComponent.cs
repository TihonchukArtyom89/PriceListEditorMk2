using Microsoft.AspNetCore.Mvc;
using PriceListEditor.Application.Models;
using PriceListEditor.Application.ViewModels;
using System.Xml.Linq;

namespace PriceListEditor.Application.Components;

public class CategoryFilterViewComponent : ViewComponent
{
    private IProductRepository productRepository;
    public CategoryFilterViewComponent(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public IViewComponentResult Invoke(ProductsListViewModel productsListViewModel)
    {
        Dictionary<string, string> productNameNumber = new Dictionary<string, string>();
        //(int)Math.Ceiling((decimal)TotalItems / PageSize)
        productNameNumber.Add("Все продукты", ((int)Math.Ceiling((decimal)productRepository.Categories.Count() / ViewBag.SelectedPageSize)).ToString());//productRepository.Categories.Count().ToString()
        for (int i = 1; i < productRepository.Categories.Count(); i++)
        {
            string categoryName = productRepository.Categories.Skip(i).First().CategoryName;
            long categoryId = (productRepository.Categories.Where(e => e.CategoryName == categoryName).FirstOrDefault() ?? new Category { CategoryID = 0 }).CategoryID ?? 0;
            productNameNumber.Add(categoryName, ((int)Math.Ceiling((decimal)productRepository.Categories.Where(e => e.CategoryID == categoryId).Count() / ViewBag.SelectedPageSize)).ToString().ToString());//productRepository.Products.Where(e => e.CategoryID == categoryId).Count().ToString()
        }
        return View(productNameNumber);
        //return View(productRepository.Categories.Select(e => e.CategoryName).OrderBy(e => e));
    }
}
