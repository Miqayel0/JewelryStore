using App.Domain.Entities;
using App.Domain.Enums;
using App.UseCase.Interfaces.Commands;
using App.UseCase.Interfaces.Repositories;
using App.UseCase.Models.Employees;
using AutoMapper;

namespace App.UseCase.Command.V1;

public class EmployeeCommand : IEmployeeCommand
{
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IOrderRepository _orderRepository;

    public EmployeeCommand(
        IMapper mapper,
        IEmployeeRepository employeeRepository,
        IOrderRepository orderRepository)
    {
        _mapper = mapper;
        _employeeRepository = employeeRepository;
        _orderRepository = orderRepository;
    }

    public async Task<int> CreateEmployeeAsync(EmployeeDto dto)
    {
        var employee = _mapper.Map<Employee>(dto);
        await _employeeRepository.AddAsync(employee);
        var orders = await _orderRepository.FindOrdersToReduceStartTimeAsync();
        if (orders.Any())
        {
            var firstOrder = orders.First();
            var reduceTime = firstOrder.StartDate - DateTime.UtcNow;
            firstOrder.Status = OrderStatus.Started;
            firstOrder.Employee = employee;
            foreach (var order in orders)
            {
                order.StartDate = order.StartDate.Add(-reduceTime);
            }

            _orderRepository.UpdateRange(orders);
        }

        await _employeeRepository.SaveChangesAsync();
        return employee.Id;
    }
}
