using capacitaciones_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace capacitaciones_api.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController(DepartamentoRepository departamentoRepository) : ControllerBase
{
    readonly DepartamentoRepository _departamentoRepository = departamentoRepository;

    [HttpGet(Name = "departments")]
    public async Task<List<Departamento>> Departments()
    {
        List<Departamento> departments = await _departamentoRepository.Departments();
        return departments;
    }

    [HttpGet("{departmentId}", Name = "departments/{departmentId}")]
    public async Task<ActionResult<Departamento>> DepartamentById(int departmentId)
    {
        Departamento department = await _departamentoRepository.DepartmentsById(departmentId);
        return department;
    }

}