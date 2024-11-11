using PriceListEditor.Application.Models;

namespace PriceListEditor.Application.ViewModels;

public class ProductsListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public PageViewModel PageViewModel { get; set; } = new() { CurrentAction= "Products" };
    public string? CurrentCategory { get; set; }
    public int[] PageSizes = new int[] { 1, 2, 3, 5, 10 };
}
