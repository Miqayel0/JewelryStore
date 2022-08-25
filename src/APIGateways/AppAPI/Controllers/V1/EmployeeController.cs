using App.UseCase.Interfaces.Commands;
using App.UseCase.Models.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers.V1;

public class EmployeeController : ApiController
{
    private readonly IEmployeeCommand _employeeCommand;

    public EmployeeController(IEmployeeCommand employeeCommand)
    {
        _employeeCommand = employeeCommand;
    }

    [HttpPost]
    [AllowAnonymous]
    public Task<int> CreateEmployee(EmployeeDto dto)
    {
        return _employeeCommand.CreateEmployeeAsync(dto);
    }
}
