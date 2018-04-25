using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Cherry.Data.Schools;

namespace Cherry.Data.Globals
{
    public class City
    {
        [Key]
        public string Name { get; set; }
    }
}
