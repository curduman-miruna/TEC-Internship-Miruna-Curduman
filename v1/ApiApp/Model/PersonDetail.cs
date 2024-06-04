﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Internship.Model
{
    public class PersonDetail
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public string PersonCity { get; set; }

        
    }
}
