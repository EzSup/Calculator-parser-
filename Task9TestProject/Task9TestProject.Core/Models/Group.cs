using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task9TestProject.Core.Models
{
    public class Group
    {
        public int Id { get; set; }

        [DisplayName("Course id")]
        public int Course_id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
