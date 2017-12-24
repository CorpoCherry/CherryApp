using System;
using System.Collections.Generic;
using System.Text;

namespace Cherry.Data.Schools
{
    public class Student
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }

        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public DateTime LastLogin { get; set; }
        public DateTime LastFullLogin { get; set; }
        public bool IsLocked { get; set; }
    }
}
