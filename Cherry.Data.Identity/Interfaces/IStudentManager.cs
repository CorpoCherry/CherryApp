using System;
using System.Collections.Generic;
using System.Text;
using Cherry.Data.School.Persons;

namespace Cherry.Data.Identity.Interfaces
{
    public interface IStudentManager
    {
        Student CurrentStudent { get; }
    }
}
