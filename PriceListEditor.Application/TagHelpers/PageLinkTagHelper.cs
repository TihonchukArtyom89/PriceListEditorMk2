using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PriceListEditor.Application.ViewModels;



namespace PriceListEditor.Application.TagHelpers;

public class PageLinkTagHelper : TagHelper
{
    private IUrlHelperFactory urlHelperFactory;
    public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        this.urlHelperFactory = urlHelperFactory;
    }
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }
    public PageViewModel? PageViewModel { get; set; }
    public string? PageAction { get; set; }
    public string? PageController { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext ?? new ViewContext());
        output.TagName = "div";        
        TagBuilder ulBuilder = new TagBuilder("ul");
        ulBuilder.AddCssClass("pagination");
        TagBuilder previousPage = CreatePreviousTag(PageViewModel!.PageNumber - 1, urlHelper, PageViewModel!.HasPreviousPage);
        ulBuilder.InnerHtml.AppendHtml(previousPage);
        for (int i = 1; i <= PageViewModel!.TotalCountOfPages; i++)
        {
            TagBuilder pageBuilder = CreateTag(i, urlHelper);
            ulBuilder.InnerHtml.AppendHtml(pageBuilder);
        }
        TagBuilder nextPage = CreateNextTag(PageViewModel.PageNumber + 1, urlHelper, PageViewModel.HasNextPage);
        ulBuilder.InnerHtml.AppendHtml(nextPage);
        output.Content.AppendHtml(ulBuilder);
    }
    TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
    {
        TagBuilder listItem = new TagBuilder("li");
        TagBuilder pageLink = new TagBuilder("a");
        if (pageNumber == this.PageViewModel!.PageNumber)
        {
            listItem.AddCssClass("active");
        }
        else
        {
            pageLink.Attributes["href"] = urlHelper.Action(action: PageAction, values: new { page = pageNumber });
        }
        listItem.AddCssClass("page-item");
        pageLink.AddCssClass("page-link");
        pageLink.InnerHtml.Append(pageNumber.ToString());
        listItem.InnerHtml.AppendHtml(pageLink);
        return listItem;
    }
    TagBuilder CreatePreviousTag(int pageNumber, IUrlHelper urlHelper,bool hasPreviousPage)
    {
        TagBuilder listItem = new TagBuilder("li");
        TagBuilder pageLink = new TagBuilder("a");
        if(hasPreviousPage)
        {
            pageLink.Attributes["href"] = urlHelper.Action(action: PageAction, values: new { page = pageNumber });
            listItem.AddCssClass("active");
        }
        listItem.AddCssClass("page-item");
        pageLink.AddCssClass("page-link");
        pageLink.InnerHtml.Append("Предыдущая");
        listItem.InnerHtml.AppendHtml(pageLink);
        return listItem;
    }
    TagBuilder CreateNextTag(int pageNumber, IUrlHelper urlHelper, bool hasNextPage)
    {
        TagBuilder listItem = new TagBuilder("li");
        TagBuilder pageLink = new TagBuilder("a");
        if (hasNextPage)
        {
            pageLink.Attributes["href"] = urlHelper.Action(action: PageAction, values: new { page = pageNumber });
            listItem.AddCssClass("active");
        }
        listItem.AddCssClass("page-item");
        pageLink.AddCssClass("page-link");
        pageLink.InnerHtml.Append("Следующая");
        listItem.InnerHtml.AppendHtml(pageLink);
        return listItem;
    }
}
