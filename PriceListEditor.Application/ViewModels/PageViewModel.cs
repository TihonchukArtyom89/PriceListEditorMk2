
namespace PriceListEditor.Application.ViewModels;

public class PageViewModel
{//PageViewModel
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public int CurrenPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}
