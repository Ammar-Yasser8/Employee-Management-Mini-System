using EmployeeManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services.Interfaces
{
    public interface IDepartmentService
    {
        public Task<IEnumerable<DepartmentResponseDto>> GetAllAsync();
        public Task<DepartmentResponseDto?> GetByIdAsync(int id);
        public Task CreateAsync(CreateDepartmentDto dto);
        public Task UpdateAsync(int id, UpdateDepartmentDto dto);
        public Task DeleteAsync(int id);
    }
}
