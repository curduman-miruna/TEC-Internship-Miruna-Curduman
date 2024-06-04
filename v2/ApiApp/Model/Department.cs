using System.ComponentModel.DataAnnotations;

namespace Internship.Model
{
    public class Department
    {
        public Department()
        {
        }
        [Key]
        public int DepartmentId { get; set; }
        [Required]
        public string DepartmentName { get; set; }

    }
}
