using Project_Tasks.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Tasks.Data
{
    public class ProjectTasksRepository : I
    {
        private readonly ProjectTasksDbContext dbContext;

        public ProjectTasksRepository(ProjectTasksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddProject(Project project)
        {
            dbContext.Projects.Add(project);
            dbContext.SaveChanges();
        }
        public IEnumerable<Project> GetAllProjects()
        {
            return dbContext.Projects.ToList();
        }
    }
}
