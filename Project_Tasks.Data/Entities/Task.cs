using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Tasks.Data.Entities
{
    public class Task
    {
        
        public int Id { get; set;  }
        public string Name { get; set;  }
        public string Description { get; set; }

        public int ProjectID { get; set; }

        public Project Project { get; set; }
    }
}
