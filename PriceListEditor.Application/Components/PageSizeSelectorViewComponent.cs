using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceListEditor.Application.Components;

public class PageSizeSelectorViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        int[] pageSizes = new int[] { 1,2,3,5,10};
        ViewBag.PageSizes = pageSizes;
        ViewBag.SelectedPageSize = pageSizes[0];
        List<SelectListItem> selectListItems = new();
        foreach (int pageSize in pageSizes)
        {
            if(pageSize == ViewBag.SelectedPageSize)
            {
                selectListItems.Add(new SelectListItem(pageSize.ToString(),pageSize.ToString(), true));
            }
            else
            {
                selectListItems.Add(new SelectListItem(pageSize.ToString(), pageSize.ToString()));
            }
        }
        return View(selectListItems);
    }
}
