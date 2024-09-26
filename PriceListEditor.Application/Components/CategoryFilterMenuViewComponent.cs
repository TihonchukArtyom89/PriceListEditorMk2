using Microsoft.AspNetCore.Mvc;

namespace PriceListEditor.Application.Components;

public class CategoryFilterMenuViewComponent :ViewComponent
{
    public string Invoke()
    {
        return "from view component";
            
    }
}
