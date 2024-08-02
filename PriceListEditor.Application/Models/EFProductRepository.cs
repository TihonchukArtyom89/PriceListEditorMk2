namespace PriceListEditor.Application.Models;

public class EFProductRepository : IProductRepository
{
    private PredpriyatieDBContext context;
    public EFProductRepository(PredpriyatieDBContext ctx)
    {
        context = ctx;
    }
    public IQueryable<Product> Products => context.Products;
}
