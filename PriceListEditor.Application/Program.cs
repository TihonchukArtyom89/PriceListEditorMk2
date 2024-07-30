var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();//set up shared objects for MVC framweork
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.UseStaticFiles();//use static content from wwwroot folder
app.MapDefaultControllerRoute();//registers MVC framework as source of endpoint by using default convention of mapping requests to classes and methods

app.Run();
