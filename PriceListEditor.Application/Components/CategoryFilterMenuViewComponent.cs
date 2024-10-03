using Microsoft.AspNetCore.Mvc;
using PriceListEditor.Application.Models;
namespace PriceListEditor.Application.Components;

public class CategoryFilterMenuViewComponent :ViewComponent
{
    private IProductRepository productRepository;
    public CategoryFilterMenuViewComponent(IProductRepository _productRepository)
    {
        productRepository = _productRepository;
    }

    public IViewComponentResult Invoke()
    {
        IOrderedQueryable<Category?>? resultQuery = (productRepository.Products ?? new List<Product>().AsQueryable()).Select(e => e.Category).Distinct().OrderBy(e => e);
        ViewBag.SelectedCategory = RouteData?.Values["category"]; 
        List<string> resultList = new List<string>();
        foreach (var s in resultQuery) 
        {
            resultList.Add((s ?? new Category() { CategoryID=0,CategoryName=string.Empty}).CategoryName);
        }
        return View(resultList as IEnumerable<string>);
    }
}
