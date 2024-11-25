using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient;

namespace PriceListEditor.Application.TagHelpers;

public class SortHeaderTagHelper : TagHelper
{
    public SortOrder CurrentValue { get; set; } //текущее значение направления сортировки для которого создаётся тэг
    public SortOrder ActiveValue { get; set; } //значение активного направления сортировки, которое было выбрано
    public string Action { get; set; } = ""; //действие контроллера для которого производится сортировка
    public string CategoryName { get; set; } = ""; //Категория продукта для сортировки
    public int PageSize { get; set; } //количество продуктов на одной странице
    public int PageNumber { get; set; } //номер страницы на котором происходит сортировка
    public bool IsAscending { get; set; } //true - если сортировка по возрастания, false если по убыванию
    private IUrlHelperFactory urlHelperFactory;
    public SortHeaderTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        this.urlHelperFactory = urlHelperFactory;
    }
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }
    public override void Process(TagHelperContext tagHelperContext, TagHelperOutput tagHelperOutput)
    {
        IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
        tagHelperOutput.TagName = "a";
        string url = urlHelper.Action(Action, new { priceSortOrder = CurrentValue, productPage = PageNumber, pageSize = PageSize, category = CategoryName }) ?? "";
        tagHelperOutput.Attributes.Add("href", url);
        if (CurrentValue == ActiveValue)//если текущее свойство имеет значение активного направления
        {
            TagBuilder iconTagBuilder = new TagBuilder("i");
            iconTagBuilder.AddCssClass("glyphicon");
            iconTagBuilder.AddCssClass(IsAscending == true ? "glyphicon-chevron-up" : "glyphicon-chevron-down");
            tagHelperOutput.PreContent.AppendHtml(iconTagBuilder);
        }
    }
}
