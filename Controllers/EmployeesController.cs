using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeesController(EmpleadoRepository empleadoRepository) : ControllerBase
{
    readonly EmpleadoRepository _empleadoRepository = empleadoRepository;

    [HttpGet(Name = "employees/")]
    public async Task<IEnumerable<Empleado>> Employees() => await _empleadoRepository.Employees();

    [HttpGet("{employeeId}", Name = "{employeeId}")]
    public async Task<ActionResult<Empleado>> EmployeeById(int employeeId)
    {
        Empleado? employee = await _empleadoRepository.EmployeeById(employeeId);

        if (employee is null)
            return NotFound();

        return employee;
    }

}