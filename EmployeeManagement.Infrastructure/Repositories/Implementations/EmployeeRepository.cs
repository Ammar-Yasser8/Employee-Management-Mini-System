using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Repositories.Implementations
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task<IEnumerable<Employee>> GetAllWithDepartmentAsync()
        {
            return await _context.Employees.Include(e => e.Department)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdWithDepartmentAsync(int id)
        {
            return await _context.Employees.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> SearchAsync(string keyword)
        {
            keyword = keyword.Trim().ToLower();
            return await _context.Employees
               .Include(e => e.Department)
               .Where(e =>
                   e.FullName.Contains(keyword) ||
                   e.Department!.Name.Contains(keyword))
               .AsNoTracking()
               .ToListAsync();

        }
    }
}
