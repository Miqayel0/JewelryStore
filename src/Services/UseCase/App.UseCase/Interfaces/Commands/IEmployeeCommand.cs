using App.UseCase.Models.Employees;

namespace App.UseCase.Interfaces.Commands;

public interface IEmployeeCommand
{
    Task<int> CreateEmployeeAsync(EmployeeDto dto);
}
