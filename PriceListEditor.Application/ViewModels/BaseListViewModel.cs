using PriceListEditor.Application.Models;


namespace PriceListEditor.Application.ViewModels;

public class BaseListViewModel
{
    public PageViewModel PageViewModel { get; set; } = new();
    public int[] PageSizes = new int[] { };
}
