namespace PriceListEditor.Tests;

public class ProductControllerTests 
{
    [Fact]
    public void Can_Use_Repository()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[] 
        {
            new Product{ProductID=1,ProductName="P1"},
            new Product{ProductID=2,ProductName="P2"}
        }).AsQueryable<Product>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        IEnumerable<Product>? result = (productController.ProductList() as ViewResult)?.ViewData.Model as IEnumerable<Product>;
        //Assert
        Product[] products = result?.ToArray() ?? Array.Empty<Product>();
        Assert.True(products.Length == 2);//Assert.Equal(2, products.Length);//одинаково
        Assert.Equal("P1", products[0].ProductName);
        Assert.Equal(1, products[0].ProductID);
        Assert.Equal("P2", products[1].ProductName);
        Assert.Equal(2, products[1].ProductID);
    }
}