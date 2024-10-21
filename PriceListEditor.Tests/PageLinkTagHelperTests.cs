using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PriceListEditor.Application.Infrastructure;
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
        Mock<IUrlHelper> mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.SetupSequence(e => e.Action(It.IsAny<UrlActionContext>())).Returns("Test/Page1").Returns("Test/Page1").Returns("Test/Page2").Returns("Test/Page3").Returns("Test/Page3");
        Mock<IUrlHelperFactory> mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
        mockUrlHelperFactory.Setup(e => e.GetUrlHelper(It.IsAny<ActionContext>())).Returns(mockUrlHelper.Object);
        Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
        PageLinkTagHelper pageLinkTagHelper = new PageLinkTagHelper(mockUrlHelperFactory.Object)
        {
            PageModel = new PagingInfo()
            {
                CurrenPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            },
            ViewContext = mockViewContext.Object,
            PageAction = "Test"
        };
        TagHelperContext tagHelperContext = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");
        Mock<TagHelperContent> mockTagHelperContent = new Mock<TagHelperContent>();
        TagHelperOutput tagHelperOutput = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(mockTagHelperContent.Object));
        //Act
        pageLinkTagHelper.Process(tagHelperContext, tagHelperOutput);
        //Assert
        Assert.Equal(@"<a class="" "" href=""Test/Page1""> &lt;- </a><a href=""Test/Page1"">1</a><a href=""Test/Page2"">2</a><a href=""Test/Page3"">3</a><a class="" "" href=""Test/Page3""> -&gt; </a>", tagHelperOutput.Content.GetContent());
    }
}
