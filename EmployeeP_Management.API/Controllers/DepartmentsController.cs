using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeP_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        // GET : get all departments
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(departments);
        }
        // Get department by id 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null) 
                return NotFound();
            return Ok(department);
        }

        // Create a new department
        [HttpPost]
        public async Task <IActionResult> Create(CreateDepartmentDto dto)
        {
           await _departmentService.CreateAsync(dto);
            return Ok();
        }
        // Updaet an existing department 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , UpdateDepartmentDto dto)
        {
            await _departmentService.UpdateAsync(id, dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
