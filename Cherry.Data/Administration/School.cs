using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Cherry.Data.Globals;
using Cherry.Data.Schools;

namespace Cherry.Data.Administration
{
    public class School
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
                _id = value;
                if (string.IsNullOrEmpty(value))
                {
                    _id = Guid.NewGuid().ToString();
                }
            }
        }
        private string _id { get; set; }

        public string OfficialName { get; set; }
        public string PseudoName { get; set; }
        public string NamedBy { get; set; }

        public City City { get; set; }
        public string Country { get; set; }
        public string Adrress { get; set; }

        [MaxLength(255)]
        public string Tag { get; set; }
    }
}
