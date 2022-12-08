using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Tasks.Domain
{
    internal class Project
    {
        //ovo je na novoj grani1
        public int Id { get;  }
        public string Name { get; }
        public string Code { get;  }

        public Project(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }
    }
}
