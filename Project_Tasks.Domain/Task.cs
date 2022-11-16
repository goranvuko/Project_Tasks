using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Tasks.Domain
{
    public class Task
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }

        public Task(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
