using App.Domain.Entities;
using App.Domain.Interfaces;
using App.Domain.Models.Employees;

namespace App.UseCase.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee, int>
{
    Task<EmployeeToAssingModel> FindEmployeeToAssignAsync();
}
