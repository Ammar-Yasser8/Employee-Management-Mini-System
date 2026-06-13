using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeP_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // Get all employees
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }
        // Get employee by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        // Search employees
        [HttpGet("search")]
        public async Task<IActionResult> SearchEmployees([FromQuery] string searchTerm)
        {
            var employees = await _employeeService.SearchAsync(searchTerm);
            return Ok(employees);
        }

        // Create a new employee
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            await _employeeService.CreateAsync(dto);

            return Ok();
        }
        // Update an existing employee
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeDto dto)
        {
            await _employeeService.UpdateAsync(id, dto);
            return NoContent();
        }
        // Delete an employee
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }

    }
}
