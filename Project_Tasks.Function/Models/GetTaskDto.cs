using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Tasks.Function.Models
{
    public class GetTaskDto : Entity
    {
        
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
    }
}
