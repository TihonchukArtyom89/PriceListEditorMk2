using PriceListEditor.Application.ViewModels;

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
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2}
        }).AsQueryable<Product>());
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        ProductsListViewModel result = productController.ProductList(null)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        Product[] products = result.Products.ToArray();
        Assert.Equal(2, products.Length);
        Assert.Equal("P1", products[0].ProductName);
        Assert.Equal(1, products[0].ProductID);
        Assert.Equal("P2", products[1].ProductName);
        Assert.Equal(2, products[1].ProductID);
    }
    [Fact]
    public void Can_Paginate()
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
        ProductController productController = new ProductController(mockRepository.Object) { PageSize = 3 };
        //Act
        ProductsListViewModel result = productController.ProductList(null, 2)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        Product[] p = result.Products.ToArray();
        Assert.True(2 == p.Length);
        Assert.Equal("P4", p[0].ProductName);
        Assert.Equal(4, p[0].ProductID);
        Assert.Equal("P5", p[1].ProductName);
        Assert.Equal(5, p[1].ProductID);
    }
    [Fact]
    public void Can_Send_Pagination_View_Model()
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
        ProductController productController = new ProductController(mockRepository.Object) { PageSize = 3 };
        //Act
        ProductsListViewModel productsListViewModel = productController.ProductList(null, 2)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        PageViewModel pageViewModel  = productsListViewModel.PageViewModel;
        Assert.Equal(2, pageViewModel.CurrenPage);
        Assert.Equal(2, pageViewModel.TotalPages);
        Assert.Equal(3, pageViewModel.ItemsPerPage);
        Assert.Equal(5, pageViewModel.TotalItems);
    }
    [Fact]
    public void Can_Filter_Products()
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
        ProductController productController = new ProductController(mockRepository.Object) { PageSize = 3 };
        //Act
        Product[] result = (productController.ProductList("C1", 1)?.ViewData.Model as ProductsListViewModel ?? new()).Products.ToArray();
        //Assert
        Assert.Equal(2, result.Length);
        Assert.True(result[0].ProductName == "P1" && result[0].ProductID == 1);
        Assert.True(result[1].ProductName == "P3" && result[1].ProductID == 3);
    }
    [Fact]
    public void Can_Get_Product_Count()
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
        ProductController productController = new ProductController(mockRepository.Object) { PageSize = 3 };
        Func<ViewResult, ProductsListViewModel?> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;
        //Act
        int? countC1 = GetModel(productController.ProductList("C1"))?.PageViewModel.TotalItems;
        int? countC2 = GetModel(productController.ProductList("C2"))?.PageViewModel.TotalItems;
        int? countC3 = GetModel(productController.ProductList("C3"))?.PageViewModel.TotalItems;
        int? countAll = GetModel(productController.ProductList(null))?.PageViewModel.TotalItems;
        //Assert
        Assert.Equal(2, countC1);
        Assert.Equal(3, countC2);
        Assert.Equal(0, countC3);
        Assert.Equal(5, countAll);
    }
}