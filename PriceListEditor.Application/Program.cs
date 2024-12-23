using Microsoft.EntityFrameworkCore;
using PriceListEditor.Application.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();//set up shared objects for MVC framweork
builder.Services.AddDbContext<PredpriyatieDBContext>(options => //allow connect to database by using connection string (defined in appsettings json)
{//where objects and data mapped with hep of context class (PredpriyatieDBContext cs)
    options.UseSqlServer(builder.Configuration["ConnectionStrings:AppConnection"]);
});
builder.Services.AddScoped<IProductRepository, EFProductRepository>();//create repository service where http requests get it`s own repository object(usual way to use EF Core)
var app = builder.Build();

app.UseStaticFiles();//use static content from wwwroot folder
//app.MapControllerRoute("catpage", "Products/{category}/{priceSortOrder}/Page{productPage:int}/Size{pageSize:int}", new { Controller = "Product", Action = "ProductList", productPage = 1, category = ""});//change url scheme, sortOrder = SortOrder.Neutral 
//app.MapControllerRoute("page", "Products/{priceSortOrder}/Page{productPage:int}/Size{pageSize:int}", new { Controller = "Product", Action = "ProductList", productPage = 1, category = "" });//change url scheme, sortOrder = SortOrder.Neutral
//app.MapControllerRoute("category", "Products/{priceSortOrder}/{category}", new { Controller = "Product", Action = "ProductList", productPage = 1, category = "" });//change url scheme
//app.MapControllerRoute("pagination", "Products/{priceSortOrder}/Page{productPage}/Size{pageSize}", new { Controller = "Product", Action = "ProductList", productPage = 1, category = "" });//change url scheme
app.MapDefaultControllerRoute();//registers MVC framework as source of endpoint by using default convention of mapping requests to classes and methods
SeedData.EnsurePopulated(app);//fill db with sample data values//dotnet ef database drop --force --context PredpriyatieDBContext
app.Run();
