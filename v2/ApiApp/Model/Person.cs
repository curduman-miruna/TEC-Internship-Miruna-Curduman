using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Internship.Model
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name  { get; set; } = string.Empty;
        [Required]
        public string Surname  { get; set; } = string.Empty;
        [Required]
        public int Age  { get; set; }
        [Required]
        public string Email  { get; set; } = string.Empty;
        [Required]
        public string Address  { get; set; } = string.Empty;
        [ForeignKey("Position")]
        public int PositionId { get; set; }
        public Position Position { get; set; } = new Position();
        [ForeignKey("Salary")]
        public int SalaryId { get; set; }
        public Salary Salary { get; set; } = new Salary();
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public DateTime BirthDay { get; set; }

    }
}
