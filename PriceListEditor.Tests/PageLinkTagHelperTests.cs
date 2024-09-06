using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PriceListEditor.Application.TagHelpers;
using PriceListEditor.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceListEditor.Tests;

public class PageLinkTagHelperTests
{
    [Fact]
    public void Can_Generate_Page_Links()
    {
        //Arrange
        var urlHelper = new Mock<IUrlHelper>();
        urlHelper.SetupSequence(x=>x.Action(It.IsAny<UrlActionContext>()))
            .Returns("/Product/ProductList?page=1")
            .Returns("/Product/ProductList?page=2")
            .Returns("/Product/ProductList?page=3");
        var urlHelperFactory = new Mock<IUrlHelperFactory>();
        urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);
        var viewContext = new Mock<ViewContext>();
        PageLinkTagHelper helper = new PageLinkTagHelper(urlHelperFactory.Object)
        {
            PageViewModel = new PageViewModel(pageNumber: 2,pageSize: 10,count: 28)
            {
                PageNumber = 2,
                TotalCountOfPages = 28
            },
            ViewContext = viewContext.Object,
            PageAction = "Test",
            PageController = "ProductController"
        };
        TagHelperContext tagHelperContext = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object,object>(), "");
        var content = new Mock<TagHelperContent>();
        TagHelperOutput tagHelperOutput = new TagHelperOutput("div", new TagHelperAttributeList(), (cache,encoder)=>Task.FromResult(content.Object));
        //Act
        helper.Process(tagHelperContext, tagHelperOutput);
        //Assert
        Assert.Equal(@"<ul class=""pagination""><li class=""active page-item""><a class=""page-link"" href=""/Product/ProductList?page=1"">Предыдущая</a></li>" +
        @"<li class=""page-item""><a class=""page-link"">1</a></li>" +
        @"<li class=""active page-item""><a class=""page-link"" href=""/Product/ProductList?page= 2"">2</a></li>" +
        @"<li class=""page-item""><a class=""page-link"" href=""/Product/ProductList?page= 3"">3</a></li>" +
        @"<li class=""active page-item""><a class=""page-link"" href=""/Product/ProductList?page=3"">Следующая</a></li></ul>", tagHelperOutput.Content.GetContent());
 //< ul class="pagination"><li class="active page-item"><a class="page-link" href="/Product/ProductList?page=1">Предыдущая</a></li>
 //       <li class="page-item"><a class="page-link" href="/Product/ProductList?page=1">1</a>
 //       </li><li class="active page-item"><a class="page-link">2</a></li>
 //       <li class="page-item"><a class="page-link" href="/Product/ProductList?page=3">3</a></li>
 //       <li class="active page-item"><a class="page-link" href="/Product/ProductList?page=3">Следующая</a></li></ul>
    }
}
