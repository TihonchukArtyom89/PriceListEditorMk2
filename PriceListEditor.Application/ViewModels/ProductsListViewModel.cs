using PriceListEditor.Application.Models;

namespace PriceListEditor.Application.ViewModels;

public class ProductsListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public PagingInfo PagingInfo { get; set; } = new();
    public long? CurrentCategory { get; set; }
    //public Category? CurrentCategory { get; set; }
}
