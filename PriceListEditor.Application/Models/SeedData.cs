using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PriceListEditor.Application.Models;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        PredpriyatieDBContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<PredpriyatieDBContext>();
        try
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            WriteLine("Exception message:\n" + ex.Message.ToString() + "Inner exception message:\n" + ex.InnerException?.Message);
        }
        if (!context.Products.Any())
        {
            //string[] CategoryName = { "Мебель", "Фрукты", "test" };
            //if (!context.Categories.Any())
            //{
            //    //code for insert sample data to table Categories(categories of product)
            //    context.Categories.AddRange(new Category { CategoryName = CategoryName[0] }, new Category { CategoryName = CategoryName[1] }, new Category { CategoryName = CategoryName[2] });
            //    context.SaveChanges();
            //}
            ////get id from table Categories
            //Category? Category_Мебель = context.Categories.Where(c => c.CategoryName == CategoryName[0]).FirstOrDefault();
            //Category? Category_Фрукты = context.Categories.Where(c => c.CategoryName == CategoryName[1]).FirstOrDefault();
            ////fill Products Table with sample data
            //context.Products.AddRange(
            //    new Product { ProductName = "Стул", ProductDescription = "Обычный стул", ProductPrice = 1547.04m, CategoryID = Category_Мебель?.CategoryID },
            //    new Product { ProductName = "Яблоко", ProductDescription = "Красное,наливное", ProductPrice = 196.67m, CategoryID = Category_Фрукты?.CategoryID },
            //    new Product { ProductName = "Слива", ProductDescription = "Спелая,садовая", ProductPrice = 378.00m, CategoryID = Category_Фрукты?.CategoryID },
            //    new Product { ProductName = "Стол", ProductDescription = "Для обеда в саду", ProductPrice = 3098.39m, CategoryID = Category_Мебель?.CategoryID },
            //    new Product { ProductName = "Груша", ProductDescription = "Можно скушать", ProductPrice = 247.07m, CategoryID = Category_Фрукты?.CategoryID },
            //    new Product { ProductName = "Стол", ProductDescription = "Компьтерный стол", ProductPrice = 15999.98m, CategoryID = Category_Мебель?.CategoryID }
            //
            context.Products.AddRange(
                new Product { ProductName = "Стул", ProductDescription = "Обычный стул", ProductPrice = 1547.04m, ProductCategory = "Мебель" },
                new Product { ProductName = "Яблоко", ProductDescription = "Красное,наливное", ProductPrice = 196.67m, ProductCategory = "Фрукты" },
                new Product { ProductName = "Слива", ProductDescription = "Спелая,садовая", ProductPrice = 378.00m, ProductCategory = "Фрукты" },
                new Product { ProductName = "Стол", ProductDescription = "Для обеда в саду", ProductPrice = 3098.39m, ProductCategory = "Мебель" },
                new Product { ProductName = "Груша", ProductDescription = "Можно скушать", ProductPrice = 247.07m, ProductCategory = "Фрукты" },
                new Product { ProductName = "Стол", ProductDescription = "Компьтерный стол", ProductPrice = 15999.98m, ProductCategory = "Мебель" }
                );
            context.SaveChanges();
        }
    }
}