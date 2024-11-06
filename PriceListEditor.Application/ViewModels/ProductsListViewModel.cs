using PriceListEditor.Application.Models;

namespace PriceListEditor.Application.ViewModels;

public class ProductsListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public PageViewModel PageViewModel { get; set; } = new();
    public string? CurrentCategory { get; set; }
}
