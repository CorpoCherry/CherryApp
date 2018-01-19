using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Cherry.Data.Administration;

namespace Cherry.Data.Schools
{
    public class SchoolClass
    {
        [Column(TypeName = "char(36)")]
        [Key]
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = Guid.NewGuid().ToString();
            }
        }
        private string _id { get; set; }

        [Column(TypeName = "char(36)")]
        public string SchoolId { get; set; }

        public string FullName { get; set; }
        public string QuickName { get; set; }

        public List<Student> Students { get; set; }
        public string OwnerID { get; set; }

        public SchoolClass(School school)
        {
            SchoolId = school.ID;
        }
    }
}
