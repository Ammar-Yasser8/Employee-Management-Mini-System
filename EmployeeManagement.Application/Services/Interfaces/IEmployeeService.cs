using EmployeeManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponseDto>> GetAllAsync();

        Task<EmployeeResponseDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateEmployeeDto dto);
        Task UpdateAsync(int id, UpdateEmployeeDto dto);
        Task DeleteAsync(int id);

        Task<IEnumerable<EmployeeResponseDto>>SearchAsync(string keyword);
      

    }
}
