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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task CreateAsync(CreateDepartmentDto dto)
        {
            var department = new Department
            {
                Name = dto.Name
            };
            await _departmentRepository.AddAsync(department);
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
                throw new KeyNotFoundException("Department not found");

            await _departmentRepository.DeleteAsync(department);

        }

        public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync()
        {
            var depatments = await _departmentRepository.GetAllAsync();
            return depatments.Select(MapToDto);
        }

        public async Task<DepartmentResponseDto?> GetByIdAsync(int id)
        {
            var department =await _departmentRepository.GetByIdAsync(id);

            if (department == null)
                return null;

            return MapToDto(department);
        }

        public async Task UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if(department is null)
            {
                throw new Exception($"Department with id {id} not found.");
            }
            department.Name = dto.Name;
            await _departmentRepository.UpdateAsync(department);
        }
        private static DepartmentResponseDto MapToDto(Department department)
        {
            return new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name
            };
        }
    }
}
