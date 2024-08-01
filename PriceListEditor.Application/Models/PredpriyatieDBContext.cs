using Microsoft.EntityFrameworkCore;

namespace PriceListEditor.Application.Models;

public class PredpriyatieDBContext : DbContext
{
    public PredpriyatieDBContext(DbContextOptions<PredpriyatieDBContext> options) : base(options) { }
    DbSet<Product> Products => Set<Product>();
}
