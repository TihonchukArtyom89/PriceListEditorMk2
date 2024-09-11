using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PriceListEditor.Application.TagHelpers;
using PriceListEditor.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PriceListEditor.Tests;

public class PageLinkTagHelperTests
{
    [Fact]
    public void Can_Generate_Page_Links()
    {
        //Arrange
        var urlHelper = new Mock<IUrlHelper>();
        urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
            .Returns("/Product/ProductList?page=1")
            .Returns("/Product/ProductList?page=1")
            .Returns("/Product/ProductList?page=3")
            .Returns("/Product/ProductList?page=3");
        var urlHelperFactory = new Mock<IUrlHelperFactory>();
        urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);
        var viewContext = new Mock<ViewContext>();
        int pageNumber = 2;
        int pageSize = 2;
        int count = 6;
        int TotalCountOfPages = (int)Math.Ceiling(count / (double)pageSize);
        PageLinkTagHelper helper = new PageLinkTagHelper(urlHelperFactory.Object)
        {
            PageViewModel = new PageViewModel(pageNumber: pageNumber, pageSize: pageSize, count: count)
            {
                PageNumber = pageNumber,
                TotalCountOfPages = (int)Math.Ceiling(count / (double)pageSize)
            },
            ViewContext = viewContext.Object,
            PageAction = "ProductList",
            PageController = "ProductController"
        };
        TagHelperContext tagHelperContext = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");
        var content = new Mock<TagHelperContent>();
        TagHelperOutput tagHelperOutput = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(content.Object));
        //Act
        helper.Process(tagHelperContext, tagHelperOutput);
        //Assert
        string expectedHTML = @"<ul class=""pagination""><li class=""active page-item""><a class=""page-link"" href=""/Product/ProductList?page=1"">Предыдущая</a></li>" +
                                @"<li class=""page-item""><a class=""page-link"" href=""/Product/ProductList?page=1"">1</a></li>" +
                                @"<li class=""active page-item""><a class=""page-link"">2</a></li>" +
                                @"<li class=""page-item""><a class=""page-link"" href=""/Product/ProductList?page=3"">3</a></li>" +
                                @"<li class=""active page-item""><a class=""page-link"" href=""/Product/ProductList?page=3"">Следующая</a></li></ul>";
        string actualHTML = System.Net.WebUtility.HtmlDecode(tagHelperOutput.Content.GetContent());
        Assert.Equal(expectedHTML, actualHTML);
    }
}
