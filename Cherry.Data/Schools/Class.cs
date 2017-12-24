using System;
using System.Collections.Generic;
using System.Text;
using Cherry.Data.Administration;

namespace Cherry.Data.Schools
{
    public class Class
    {
        public int ID { get; set; }

        public string FullName { get; set; }
        public string QuickName { get; set; }

        public List<Student> Students { get; set; }
        public string OwnerID { get; set; }
    }
}
