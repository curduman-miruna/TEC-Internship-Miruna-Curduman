using System.ComponentModel.DataAnnotations;

namespace Internship.Model
{
    public class Salary
    {
        [Key]
        public int SalaryId { get; set; }
        public int Amount { get; set; }
    }
}
