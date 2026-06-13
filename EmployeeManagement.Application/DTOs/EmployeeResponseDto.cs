using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.DTOs
{
    public class EmployeeResponseDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; }
    }
}
