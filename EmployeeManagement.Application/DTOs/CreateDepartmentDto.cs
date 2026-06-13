using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.DTOs
{
    public class CreateDepartmentDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
