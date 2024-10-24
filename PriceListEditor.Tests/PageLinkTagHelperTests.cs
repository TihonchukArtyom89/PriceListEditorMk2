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
        Mock<IUrlHelper> mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.SetupSequence(e => e.Action(It.IsAny<UrlActionContext>())).Returns("Test/Page1").Returns("Test/Page1").Returns("Test/Page2").Returns("Test/Page3").Returns("Test/Page3");
        Mock<IUrlHelperFactory> mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
        mockUrlHelperFactory.Setup(e=>e.GetUrlHelper(It.IsAny<ActionContext>())).Returns(mockUrlHelper.Object);
        Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
        PageLinkTagHelper pageLinkTagHelper = new PageLinkTagHelper(mockUrlHelperFactory.Object) 
        {
            PageModel = new PagingInfo() 
            {
                PageNumber = pageNumber,
                TotalCountOfPages = (int)Math.Ceiling(count / (double)pageSize)
            },
            ViewContext = viewContext.Object,
            PageAction = "ProductList",
            PageController = "ProductController"
        };
        TagHelperContext tagHelperContext = new TagHelperContext(new TagHelperAttributeList(),new Dictionary<object,object>(),"");
        Mock<TagHelperContent> mockTagHelperContent = new Mock<TagHelperContent>();  
        TagHelperOutput tagHelperOutput = new TagHelperOutput("div",new TagHelperAttributeList(),(cache,encoder)=> Task.FromResult(mockTagHelperContent.Object));
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
