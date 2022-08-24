using App.Domain.Entities;
using App.Infra.Data.Common;
using App.UseCase.Interfaces.Repositories;

namespace App.Infra.Data.Repositories;

internal class EmployeeRepository : RepositoryBase<Employee, int>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }
}
