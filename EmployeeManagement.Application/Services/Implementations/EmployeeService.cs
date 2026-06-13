using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }
        public async Task CreateAsync(CreateEmployeeDto dto)
        {
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                throw new Exception("Department not found");
            var employee = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                MobileNumber = dto.MobileNumber,
                DepartmentId = dto.DepartmentId,
                JobTitle = dto.JobTitle,
                HireDate = dto.HireDate,
                IsActive = dto.IsActive
            };
            await _employeeRepository.AddAsync(employee);
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new Exception("Employee not found");
            await _employeeRepository.DeleteAsync(employee);
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllWithDepartmentAsync();
            return employees.Select(MapToDto);

        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            var employee =
                await _employeeRepository.GetByIdWithDepartmentAsync(id);

            if (employee == null)
                return null;

            return MapToDto(employee);
        }

        public async Task<IEnumerable<EmployeeResponseDto>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Enumerable.Empty<EmployeeResponseDto>();
            }
            var employees =
                await _employeeRepository.SearchAsync(keyword);

            return employees.Select(MapToDto);
        }

        public async Task UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdWithDepartmentAsync(id);
            if (employee == null)
                throw new Exception("Employee not found");
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                throw new Exception("Department not found");
            employee.FullName = dto.FullName;
            employee.Email = dto.Email;
            employee.MobileNumber = dto.MobileNumber;
            employee.DepartmentId = dto.DepartmentId;
            employee.JobTitle = dto.JobTitle;
            employee.HireDate = dto.HireDate;
            employee.IsActive = dto.IsActive;
            await _employeeRepository.UpdateAsync(employee);

        }


        private static EmployeeResponseDto MapToDto(Employee employee)
        {
            return new EmployeeResponseDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                MobileNumber = employee.MobileNumber,
                DepartmentName = employee.Department?.Name ?? "N/A",
                JobTitle = employee.JobTitle,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive
            };
        }
    }
}
    
