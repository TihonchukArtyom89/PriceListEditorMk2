using Microsoft.EntityFrameworkCore;
using Moq;
using PriceListEditor.Application.Controllers;
using PriceListEditor.Application.Models;
using PriceListEditor.Application.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Xml;

namespace PriceListEditor.Tests;

public class ProductControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        //Arrange
        Product[] productsTestData = new Product[]
        {
            new Product{ProductID=1,ProductName="P1"},
            new Product{ProductID=2,ProductName="P2"}
        };
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns(productsTestData.AsQueryable<Product>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        Product[]? resultProducts = (productController.ProductList(null)?.ViewData.Model as ProductListViewModel ?? new())?.Products?.ToArray() ?? Array.Empty<Product>();
        //Assert
        Assert.True(resultProducts.Length == 2);
        Assert.Equal("P1", resultProducts[0].ProductName);
        Assert.Equal(1, resultProducts[0].ProductID);
        Assert.Equal("P2", resultProducts[1].ProductName);
        Assert.Equal(2, resultProducts[1].ProductID);
    }
    [Fact]
    public void Can_Paginate()
    {
        //Arrange
        Product[] productsTestData = new Product[]
        {
            new Product{ProductID=1,ProductName="P1"},
            new Product{ProductID=2,ProductName="P2"},
            new Product{ProductID=3,ProductName="P3"},
            new Product{ProductID=4,ProductName="P4"},
            new Product{ProductID=5,ProductName="P5"}
        };
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns(productsTestData.AsQueryable<Product>());
        ProductController productController = new ProductController(mockRepository.Object)
        {
            pageSize = 3
        };
        //Act
        Product[]? resultProducts = (productController.ProductList(null, 2)?.ViewData.Model as ProductListViewModel ?? new())?.Products?.ToArray() ?? Array.Empty<Product>();
        //Assert
        Assert.True(resultProducts.Length == 2);
        Assert.Equal("P4", resultProducts[0].ProductName);
        Assert.Equal("P5", resultProducts[1].ProductName);
    }
    [Fact]
    public void Can_Send_Pagination_View_Model()
    {
        //Arrange
        Product[] productsTestData = new Product[]
        {
            new Product{ProductID=1,ProductName="P1"},
            new Product{ProductID=2,ProductName="P2"},
            new Product{ProductID=3,ProductName="P3"},
            new Product{ProductID=4,ProductName="P4"},
            new Product{ProductID=5,ProductName="P5"},
            new Product{ProductID=6,ProductName="P6"},
            new Product{ProductID=7,ProductName="P7"}
        };
        int currentPage = 2;
        int pageSize = 3;
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns(productsTestData.AsQueryable<Product>());
        ProductController productController = new ProductController(mockRepository.Object)
        {
            pageSize = pageSize,
        };
        //Act
        ProductListViewModel result = productController.ProductList(null, currentPage)?.ViewData.Model as ProductListViewModel ?? new();
        //Assert
        PageViewModel actualPageViewModel = result.PageViewModel!;
        PageViewModel expectedPageViewModel = new PageViewModel(pageNumber: currentPage, pageSize: pageSize, count: productsTestData.Count());
        Assert.Equal(expectedPageViewModel.HasPreviousPage, actualPageViewModel.HasPreviousPage);
        Assert.Equal(expectedPageViewModel.HasNextPage, actualPageViewModel.HasNextPage);
        Assert.Equal(expectedPageViewModel.TotalCountOfPages, actualPageViewModel.TotalCountOfPages);
        Assert.Equal(expectedPageViewModel.PageNumber, actualPageViewModel.PageNumber);
    }
    [Fact]
    public void Can_Filter_Products()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        Category[] categoriesTestData = new Category[]
        {
            new Category{CategoryID=1,CategoryName="C1"},
            new Category{CategoryID=2,CategoryName="C1"},
            new Category{CategoryID=3,CategoryName="C3"},
        };
        Product[] productsTestData = new Product[]
        {
            new Product{ProductID=1,ProductName="P1",CategoryID=1},
            new Product{ProductID=2,ProductName="P2",CategoryID=2},
            new Product{ProductID=3,ProductName="P3",CategoryID=2},
            new Product{ProductID=4,ProductName="P4",CategoryID=1},
            new Product{ProductID=5,ProductName="P5",CategoryID=3},
            new Product{ProductID=6,ProductName="P6",CategoryID=1},
            new Product{ProductID=7,ProductName="P7",CategoryID=3}
        };
        mockRepository.Setup(mr => mr.Products).Returns(productsTestData.AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns(categoriesTestData.AsQueryable<Category>());
        ProductController productController = new ProductController(mockRepository.Object)
        {
            pageSize = 3,
        };
        //Act
        Product[] result = ((productController.ProductList("C3", 1)?.ViewData.Model as ProductListViewModel ?? new()).Products ?? new Product[] { }).ToArray();
        //Assert
        Assert.Equal(2,result.Length);
        Assert.True(result[0].ProductID==5 && result[0].ProductName == "P5" && result[0].CategoryID == 3);
        Assert.True(result[1].ProductID == 7 && result[1].ProductName == "P7" && result[1].CategoryID == 3);
    }
    [Fact]
    public void Can_Correct_Page_Count()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
            new Category{CategoryID = 1, CategoryName = "C1" },
            new Category{CategoryID = 2, CategoryName = "C2" },
            new Category{CategoryID = 3, CategoryName = "C3" },
        }).AsQueryable<Category>());
        ProductController productController = new ProductController(mockRepository.Object) { PageSize = 2 };
        Func<ViewResult, ProductsListViewModel?> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;
        //Act
        int? countC1 = GetModel(productController.ProductList("C1"))?.PagingInfo.TotalItems;
        int? countC2 = GetModel(productController.ProductList("C2"))?.PagingInfo.TotalItems;
        int? countC3 = GetModel(productController.ProductList("C3"))?.PagingInfo.TotalItems;
        int? countAll = GetModel(productController.ProductList(null))?.PagingInfo.TotalItems;
        //Assert
        Assert.Equal(1, countC1);
        Assert.Equal(2, countC2);
        Assert.Equal(0, countC3);
        Assert.Equal(3, countAll);
    }
}