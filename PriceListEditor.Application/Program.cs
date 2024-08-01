using Microsoft.EntityFrameworkCore;
using PriceListEditor.Application.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();//set up shared objects for MVC framweork
builder.Services.AddDbContext<PredpriyatieDBContext>(options => //allow connect to database by using connection string (defined in appsettings json)
{//where objects and data mapped with hep of context class (PredpriyatieDBContext cs)
    options.UseSqlServer(builder.Configuration["ConnectionStrings:AppConnection"]);
});
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.UseStaticFiles();//use static content from wwwroot folder
app.MapDefaultControllerRoute();//registers MVC framework as source of endpoint by using default convention of mapping requests to classes and methods

app.Run();
