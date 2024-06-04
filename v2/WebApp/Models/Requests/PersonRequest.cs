using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Requests
{
    public class PersonRequest
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public Position Position { get; set; }
        public int PositionId { get; set; }
        public Salary Salary { get; set; }
        public int SalaryId { get; set; }
        public string City { get; set; }
        public DateTime BirthDay { get; set; }

        public PersonRequest(Person person)
        {
            Id = person.Id;
            Name = person.Name;
            Surname = person.Surname;
            Age = person.Age;
            Email = person.Email;
            Address = person.Address;
            Position = new Position();
            Position.Department = new Department();
            PositionId = person.PositionId;
            Salary = new Salary();
            SalaryId = person.SalaryId;
            City = person.City;
            BirthDay = person.BirthDay;
        }
    }
}
