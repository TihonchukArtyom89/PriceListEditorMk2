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
        ProductsListViewModel result = productController.ProductList(null, SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new();
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
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        ProductsListViewModel result = productController.ProductList(null, SortOrder.PriceDesc, 2, 3)?.ViewData.Model as ProductsListViewModel ?? new();
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
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        ProductsListViewModel productsListViewModel = productController.ProductList(null, SortOrder.PriceDesc, 2, 3)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        PageViewModel pageViewModel = productsListViewModel.PageViewModel;
        Assert.Equal(2, pageViewModel.CurrenPage);
        Assert.Equal(2, pageViewModel.TotalPages);
        Assert.Equal(3, pageViewModel.PageSize);
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
        ProductController productController = new ProductController(mockRepository.Object);
        //Act
        Product[] result = (productController.ProductList("C1", SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).Products.ToArray();
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
        ProductController productController = new ProductController(mockRepository.Object);
        Func<ViewResult, ProductsListViewModel?> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;
        //Act
        int? countC1 = GetModel(productController.ProductList("C1", SortOrder.PriceDesc, 1, 3))?.PageViewModel.TotalItems;
        int? countC2 = GetModel(productController.ProductList("C2", SortOrder.PriceDesc, 1, 3))?.PageViewModel.TotalItems;
        int? countC3 = GetModel(productController.ProductList("C3", SortOrder.PriceDesc, 1, 3))?.PageViewModel.TotalItems;
        int? countAll = GetModel(productController.ProductList(null, SortOrder.PriceDesc, 1, 3))?.PageViewModel.TotalItems;
        //Assert
        Assert.Equal(2, countC1);
        Assert.Equal(3, countC2);
        Assert.Equal(0, countC3);
        Assert.Equal(5, countAll);
    }
    [Fact]
    public void Can_Get_Page_Size()
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
        ProductController productController = new ProductController(mockRepository.Object);
        Func<ViewResult, ProductsListViewModel?> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;
        //Act
        int? pageCount = (productController.ProductList("C1", SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.PageSize;
        //Assert
        Assert.Equal(3, pageCount);
    }
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
        int result = (productController.ProductList(null, SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.TotalPages;
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
        int result = (productController.ProductList("C2", SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageViewModel.TotalPages;
        //Assert
        Assert.Equal(2, result);
    }
    [Fact]
    public void Can_Select_Page_Sizes()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 1, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 5, ProductName = "P5", CategoryID = 3},
            new Product{ProductID = 6, ProductName = "P6", CategoryID = 1},
            new Product{ProductID = 7, ProductName = "P7", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
                new Category{CategoryID = 1, CategoryName = "C1" },
                new Category{CategoryID = 2, CategoryName = "C2" },
                new Category{CategoryID = 3, CategoryName = "C3" }
        }).AsQueryable<Category>());
        ProductController productController = new(mockRepository.Object);
        //Act
        int[] results = (productController.ProductList("C2", SortOrder.PriceDesc, 1, 3)?.ViewData.Model as ProductsListViewModel ?? new()).PageSizes;
        //Assert
        Assert.Equal(new int[] { 1, 2, 3, 5, 10 }, results);
    }
    [Fact]
    public void Can_Right_Sort_Product_On_Name()
    {
        //Arrange
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(mr => mr.Products).Returns((new Product[]
        {
            new Product{ProductID = 7, ProductName = "P1", CategoryID = 1},
            new Product{ProductID = 4, ProductName = "P2", CategoryID = 2},
            new Product{ProductID = 3, ProductName = "P3", CategoryID = 1},
            new Product{ProductID = 2, ProductName = "P4", CategoryID = 2},
            new Product{ProductID = 6, ProductName = "P5", CategoryID = 3},
            new Product{ProductID = 5, ProductName = "P6", CategoryID = 1},
            new Product{ProductID = 1, ProductName = "P7", CategoryID = 2}
        }).AsQueryable<Product>());
        mockRepository.Setup(mr => mr.Categories).Returns((new Category[]
        {
                new Category{CategoryID = 1, CategoryName = "C1" },
                new Category{CategoryID = 2, CategoryName = "C2" },
                new Category{CategoryID = 3, CategoryName = "C3" }
        }).AsQueryable<Category>());
        ProductController productController = new(mockRepository.Object);
        //Act
        ProductsListViewModel? resultNameAsc = productController.ProductList(null, SortOrder.NameAsc, 1, 10)?.ViewData.Model as ProductsListViewModel ?? new();
        ProductsListViewModel? resultNameDesc = productController.ProductList(null, SortOrder.NameDesc, 1, 10)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert
        Assert.Equal("P1", resultNameAsc.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P2", resultNameAsc.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("P3", resultNameAsc.Products.Skip(2).FirstOrDefault()!.ProductName);
        Assert.Equal("P4", resultNameAsc.Products.Skip(3).FirstOrDefault()!.ProductName);
        Assert.Equal("P5", resultNameAsc.Products.Skip(4).FirstOrDefault()!.ProductName);
        Assert.Equal("P6", resultNameAsc.Products.Skip(5).FirstOrDefault()!.ProductName);
        Assert.Equal("P7", resultNameAsc.Products.Skip(6).FirstOrDefault()!.ProductName);
        Assert.Equal("P7", resultNameDesc.Products.FirstOrDefault()!.ProductName);
        Assert.Equal("P6", resultNameDesc.Products.Skip(1).FirstOrDefault()!.ProductName);
        Assert.Equal("P5", resultNameDesc.Products.Skip(2).FirstOrDefault()!.ProductName);
        Assert.Equal("P4", resultNameDesc.Products.Skip(3).FirstOrDefault()!.ProductName);
        Assert.Equal("P3", resultNameDesc.Products.Skip(4).FirstOrDefault()!.ProductName);
        Assert.Equal("P2", resultNameDesc.Products.Skip(5).FirstOrDefault()!.ProductName);
        Assert.Equal("P1", resultNameDesc.Products.Skip(6).FirstOrDefault()!.ProductName);
    }
}