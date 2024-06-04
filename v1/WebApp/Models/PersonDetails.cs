using System;

namespace WebApp.Models
{
    public class PersonDetails
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public DateTime BirthDay { get; set; }
        public string PersonCity { get; set; }

        public PersonDetails(int personId, Person person)
        {
            Person = person;
            PersonId = personId;
        }

        public PersonDetails()
        {
        }
    }
}
