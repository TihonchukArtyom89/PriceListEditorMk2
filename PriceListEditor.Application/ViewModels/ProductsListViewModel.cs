using PriceListEditor.Application.Models;

namespace PriceListEditor.Application.ViewModels;

public class ProductsListViewModel : BaseListViewModel
{
    public ProductsListViewModel()
    {
        PageViewModel = new() { Pseudonym = "Products" };
        PageSizes = new int[] { 1, 2, 3, 5, 10 };
    }
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    //public PageViewModel PageViewModel { get; set; } = new() { Pseudonym = "Products" };
    public string? CurrentCategory { get; set; }
    //public int[] PageSizes = new int[] { 1, 2, 3, 5, 10 };
}
