using PriceListEditor.Application.ViewModels;

namespace PriceListEditor.Tests;

public class ProductControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        //mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        //{
        //    new Product{ProductID=1,ProductName="P1"},
        //    new Product{ProductID=2,ProductName="P2"}
        //}).AsQueryable<Product>());//for synchronous variat of ProductList method in ProductController
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID=1,ProductName="P1"},
            new Product{ProductID=2,ProductName="P2"}
        }).AsQueryable<Product>());//for asynchronous variat of ProductList method in ProductController
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        //var resultProducts = (productController.ProductList()?.ViewData.Model as ProductListViewModel ?? new())?.Products?.ToArray() ?? Array.Empty<Product>();//for synchronous variat of ProductList method in ProductController
        var resultProducts = (productController.ProductList()?.Result as ProductListViewModel ?? new())?.Products?.ToArray() ?? Array.Empty<Product>();//перерработать тест не проходитс€
        //(productController.ProductList()?.ViewData.Model as ProductListViewModel ?? new())?.Products?.ToArray() ?? Array.Empty<Product>();
        //Assert
        Assert.True(resultProducts.Length == 2);//Assert.Equal(2, products.Length);//одинаково
        Assert.Equal("P1", resultProducts[0].ProductName);
        Assert.Equal(1, resultProducts[0].ProductID);
        Assert.Equal("P2", resultProducts[1].ProductName);
        Assert.Equal(2, resultProducts[1].ProductID);
    }
}