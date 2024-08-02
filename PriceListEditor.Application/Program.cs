using Microsoft.EntityFrameworkCore;
using PriceListEditor.Application.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();//set up shared objects for MVC framweork
builder.Services.AddDbContext<PredpriyatieDBContext>(options => //allow connect to database by using connection string (defined in appsettings json)
{//where objects and data mapped with hep of context class (PredpriyatieDBContext cs)
    options.UseSqlServer(builder.Configuration["ConnectionStrings:AppConnection"]);
});
builder.Services.AddScoped<IProductRepository,EFProductRepository>();//create repository service where http requests get it`s own repository object(usual way to use EF Core)
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.UseStaticFiles();//use static content from wwwroot folder
app.MapDefaultControllerRoute();//registers MVC framework as source of endpoint by using default convention of mapping requests to classes and methods

app.Run();
