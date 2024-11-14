using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceListEditor.Application.ViewModels;

namespace PriceListEditor.Application.Components;

public class PageSizeSelectorViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(BaseListViewModel listViewModel)
    {
        ViewBag.SelectedPageSize = listViewModel.PageViewModel.PageSize;
        return View(listViewModel);
    }
}
