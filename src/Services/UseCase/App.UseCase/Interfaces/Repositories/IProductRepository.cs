using App.Domain.Entities;
using App.Domain.Interfaces;

namespace App.UseCase.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product, int>
{
}
