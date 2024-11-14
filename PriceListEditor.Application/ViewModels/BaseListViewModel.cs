using PriceListEditor.Application.Models;


namespace PriceListEditor.Application.ViewModels;

public class BaseListViewModel
{
    public PageViewModel PageViewModel { get; set; } = new();
    public bool IsEmpty { get; set; }
    public int[] PageSizes = new int[] { };
}
