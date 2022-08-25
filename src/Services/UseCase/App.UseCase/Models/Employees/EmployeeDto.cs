using App.Domain.Entities;
using App.UseCase.Common;
using System.ComponentModel.DataAnnotations;

namespace App.UseCase.Models.Employees;

public class EmployeeDto : IAutoMap<Employee, EmployeeDto>
{
    [Required]
    public string Name { get; set; }
}
