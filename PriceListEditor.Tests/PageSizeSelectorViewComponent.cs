using Microsoft.AspNetCore.Mvc.ViewComponents;
using PriceListEditor.Application.Components;
using PriceListEditor.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceListEditor.Tests;

public class PageSizeSelectorViewComponent
{
    [Fact]
    public void Can_Get_Page_Count_Of_All_Products()
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
                new Category{CategoryID = 2, CategoryName = "C2" }
        }).AsQueryable<Category>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        int result = (productController.ProductList(null, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.TotalPages;
        //Assert
        Assert.Equal(2, result);
    }
    [Fact]
    public void Can_Get_Page_Count_Of_Specific_Products()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 2},
            new Product{ProductID = 6, ProductName = "P6", CategoryID = 1},
            new Product{ProductID = 7, ProductName = "P7", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
                new Category{CategoryID = 1, CategoryName = "C1" },
                new Category{CategoryID = 2, CategoryName = "C2" }
        }).AsQueryable<Category>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        int result = (productController.ProductList("C2", 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.TotalPages;
        //Assert
        Assert.Equal(2, result);
    }
    [Fact]
    public void Can_Select_Page_Sizes()
    {
        //Arrange
        //Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        //mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        //{
        //    new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
        //    new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
        //    new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
        //    new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
        //    new Product{ProductID = 5, ProductName = "P5", CategoryID = 3},
        //    new Product{ProductID = 6, ProductName = "P6", CategoryID = 1},
        //    new Product{ProductID = 7, ProductName = "P7", CategoryID = 2}
        //}).AsQueryable<Product>());
        //mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        //{
        //        new Category{CategoryID = 1, CategoryName = "C1" },
        //        new Category{CategoryID = 2, CategoryName = "C2" },
        //        new Category{CategoryID = 3, CategoryName = "C3" }
        //}).AsQueryable<Category>());
        BaseListViewModel baseListViewModel = new BaseListViewModel() { PageSizes = new int[]{ 1, 2, 5} };
        //PageSizeSelectorViewComponent pageSizeSelectorViewComponent = new PageSizeSelectorViewComponent();
        //Act
        //string[] results =  pageSizeSelectorViewComponent
        //Assert
    }
}
