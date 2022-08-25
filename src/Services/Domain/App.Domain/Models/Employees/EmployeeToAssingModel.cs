using App.Domain.Interfaces;

namespace App.Domain.Models.Employees;

public class EmployeeToAssingModel : IEntity
{
    public int Id { get; set; }
    public bool Assigned { get; set; }
    public DateTime FinishDate { get; set; }
}
