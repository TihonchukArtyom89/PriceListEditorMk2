using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceListEditor.Application.ViewModels;

namespace PriceListEditor.Application.Components;

public class PageSizeSelectorViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ProductsListViewModel productsList)
    {
        ViewBag.SelectedPageSize = productsList.PageViewModel.PageSize;
        ViewBag.PageAction = productsList.PageViewModel.CurrentAction;
        return View(productsList);
    }
}
