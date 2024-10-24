﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NuGet.Protocol;
using PriceListEditor.Application.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace PriceListEditor.Tests
{
    public class CategoryFilterViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
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
            CategoryFilterViewComponent categoryFilterViewComponent = new CategoryFilterViewComponent(mockRepository.Object);
            //Act
            string[] set_of_categories = ((IEnumerable<string>?)(categoryFilterViewComponent.Invoke() as ViewViewComponentResult)?.ViewData?.Model ?? Enumerable.Empty<string>()).ToArray();
            //Assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "C1", "C2" }, set_of_categories));
        }
        [Fact]
        public void Show_Selected_Category()
        {
            //Arrange
            string selectedCategory = "C2";
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
            CategoryFilterViewComponent categoryFilterViewComponent = new CategoryFilterViewComponent(mockRepository.Object);
            categoryFilterViewComponent.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };
            categoryFilterViewComponent.RouteData.Values["category"] = selectedCategory;
            //Act
            string getSelectedCategory = (string?)(categoryFilterViewComponent.Invoke() as ViewViewComponentResult)?.ViewData?["SelectedCategory"] ?? "";
            //Assert
            Assert.Equal(selectedCategory, getSelectedCategory);
        }
    }
}
