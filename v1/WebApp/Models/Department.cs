using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Department
    {
        public int DepartmentId { get; set; } = 0;
        public string DepartmentName { get; set; } = string.Empty;
    }
}
