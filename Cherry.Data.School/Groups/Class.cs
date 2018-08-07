using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cherry.Data.School.Persons;

namespace Cherry.Data.School.Groups
{
    public class Class
    {
        [Column(TypeName = "char(36)")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string FullName { get; set; }
        public string ShortName { get; set; }

        public List<Student> Students { get; set; }
    }
}
