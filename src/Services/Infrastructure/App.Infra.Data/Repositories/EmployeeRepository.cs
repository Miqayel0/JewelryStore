using App.Domain.Entities;
using App.Domain.Models.Employees;
using App.Infra.Data.Common;
using App.UseCase.Interfaces.Repositories;
using Dapper;
using System.Data;

namespace App.Infra.Data.Repositories;

internal class EmployeeRepository : RepositoryBase<Employee, int>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }

    public Task<EmployeeToAssingModel> FindEmployeeToAssignAsync()
    {
        return DbConnection.QueryFirstOrDefaultAsync<EmployeeToAssingModel>(
            "[sp_getEmployeeToAssign]",
            commandType: CommandType.StoredProcedure);
    }
}
