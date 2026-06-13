using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Repositories.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IEnumerable<Employee>>SearchAsync(string keyword);

        Task<IEnumerable<Employee>>GetAllWithDepartmentAsync();

        Task<Employee?>GetByIdWithDepartmentAsync(int id);

    }
}
