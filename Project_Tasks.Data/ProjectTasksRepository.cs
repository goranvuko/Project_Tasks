using Project_Tasks.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Tasks.Data
{
    public class ProjectTasksRepository : IProjectTasksRepository
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
        public Project GetProject(int id)
        {
            return dbContext.Projects.SingleOrDefault(p => p.Id == id);
        }
        public IEnumerable<Project> GetAllProjects()
        {
            return dbContext.Projects.ToList();
        }
    }
}
