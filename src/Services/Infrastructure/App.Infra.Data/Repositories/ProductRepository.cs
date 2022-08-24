using App.Domain.Entities;
using App.Infra.Data.Common;
using App.UseCase.Interfaces.Repositories;

namespace App.Infra.Data.Repositories;

public class ProductRepository : RepositoryBase<Product, int>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
}
