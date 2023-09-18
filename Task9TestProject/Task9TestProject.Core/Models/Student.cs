using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task9TestProject.Core.Models
{
    public class Student
    {
        public int Id { get; set; }

        [DisplayName("Group id")]
        public int Group_Id { get; set; }

        [DisplayName("FirstName")]
        public string FirstName { get; set; }

        [DisplayName("LastName")]
        public string LastName { get; set; }

    }
}
