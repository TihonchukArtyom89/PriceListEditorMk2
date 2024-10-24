using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PriceListEditor.Application.ViewModels;
using System.Runtime.CompilerServices;

namespace PriceListEditor.Application.Infrastructure;

    [HtmlTargetElement("div",Attributes = "page-model")]
public class PageLinkTagHelper : TagHelper
{
    private IUrlHelperFactory urlHelperFactory;
    public PageLinkTagHelper (IUrlHelperFactory _urlHelperFactory)
    {
        urlHelperFactory = _urlHelperFactory;
    }
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }
    public PagingInfo ? PageModel {  get; set; }
    public string? PageAction {  get; set; }
    [HtmlAttributeName(DictionaryAttributePrefix ="page-url-")]
    public Dictionary<string,object> PageUrlValues { get; set; }= new Dictionary<string,object>();
    public bool PageClassEnabled { get; set; } = false;
    public string PageClass { get; set; } = string.Empty;
    public string PageClassNormal { get; set; } = string.Empty;
    public string PageClassSelected { get; set; } = string.Empty;
    public string PageClassArrow { get; set; } = string.Empty;
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext != null && PageModel != null)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("a");
            TagBuilder tag = new TagBuilder("a");
            tag.Attributes["href"] = urlHelper.Action(PageAction, new { productPage = PageModel.CurrenPage == 1 ? 1 : PageModel.CurrenPage - 1 });
            tag.AddCssClass(PageClass);
            tag.AddCssClass(PageClassArrow);
            tag.InnerHtml.Append(" <- ");
            result.InnerHtml.AppendHtml(tag);
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                tag = new TagBuilder("a");
                PageUrlValues["productPage"] = i;
                tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                if (PageClassEnabled) 
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i==PageModel.CurrenPage ? PageClassSelected : PageClassNormal);
                }
                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag);
            }
            tag = new TagBuilder("a");
            tag.Attributes["href"] = urlHelper.Action(PageAction, new { productPage = PageModel.CurrenPage == PageModel.TotalPages ? PageModel.TotalPages : PageModel.CurrenPage + 1 });
            tag.AddCssClass(PageClass);
            tag.AddCssClass(PageClassArrow);
            tag.InnerHtml.Append(" -> ");
            result.InnerHtml.AppendHtml(tag);
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
