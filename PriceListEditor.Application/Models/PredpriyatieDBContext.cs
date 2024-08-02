using Microsoft.EntityFrameworkCore;

namespace PriceListEditor.Application.Models;

public class PredpriyatieDBContext : DbContext
{
    public PredpriyatieDBContext(DbContextOptions<PredpriyatieDBContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();
}
