using App.Domain.Entities;
using App.Infra.Data.Common;
using App.UseCase.Interfaces.Repositories;

namespace App.Infra.Data.Repositories;

internal class CollectionRepository : RepositoryBase<Collection, int>, ICollectionRepository
{
    public CollectionRepository(AppDbContext context) : base(context)
    {
    }
}
