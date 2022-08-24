using App.Domain.Entities;
using App.Domain.Interfaces;

namespace App.UseCase.Interfaces.Repositories;

public interface ICollectionRepository : IRepository<Collection, int>
{
}
