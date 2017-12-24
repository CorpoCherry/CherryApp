using System;
using System.Collections.Generic;
using System.Text;
using Cherry.Data.Schools;

namespace Cherry.Data
{
    public class School
    {
        public int ID { get; set; }
        
        public string Name { get; set; }
        public List<Class> Classes { get; set; }
    }
}
