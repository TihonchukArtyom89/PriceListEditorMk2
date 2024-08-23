using Microsoft.AspNetCore.Mvc;
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
    public string? PageController {  get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext ?? new ViewContext());
        output.TagName = "div";
        //list of page links is unsorted - ul
        TagBuilder tag = new TagBuilder("ul");
        tag.AddCssClass("pagination");
        //create three links on next,current and previous pages
        TagBuilder currentPage = CreateTag(PageViewModel!.PageNumber , urlHelper);//current
        tag.InnerHtml.AppendHtml(currentPage);
        if (PageViewModel.HasPreviousPage) //previous
        {
            TagBuilder previousPage = CreateTag(PageViewModel.PageNumber - 1, urlHelper);
            tag.InnerHtml.AppendHtml(previousPage);
        }
        if (PageViewModel.HasNextPage) //next
        {
            TagBuilder nextPage = CreateTag(PageViewModel.PageNumber + 1, urlHelper);
            tag.InnerHtml.AppendHtml(nextPage);
        }
    }
    TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
    {
        TagBuilder listItem = new TagBuilder("li");
        TagBuilder pageLink = new TagBuilder("a");
        if(pageNumber == this.PageViewModel!.PageNumber) 
        {
            listItem.AddCssClass("active");
        }
        else
        {
            pageLink.Attributes["href"] = urlHelper.Action(action: PageAction,values: new { page = pageNumber },controller:PageController);
        }
        listItem.AddCssClass("page-item");
        pageLink.AddCssClass("page-link");
        pageLink.InnerHtml.Append(pageNumber.ToString());
        listItem.InnerHtml.AppendHtml(pageLink);
        return listItem;
    }
}
