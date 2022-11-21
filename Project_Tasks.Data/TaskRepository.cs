using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Tasks.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ProjectTasksDbContext dbContext;

        public TaskRepository(ProjectTasksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Entities.Task AddTask(Entities.Task task)
        {
            dbContext.Tasks.Add(task);
            dbContext.SaveChanges();
            return task;
        }
        public Entities.Task GetTask(int id)
        {
            return dbContext.Tasks.SingleOrDefault(t=> t.Id == id);
        }

        public void DeleteTask(int id)
        {
            dbContext.Tasks.Remove(GetTask(id));
            dbContext.SaveChanges();
        }
        public IEnumerable<Entities.Task> GetAllTasks()
        {
            return dbContext.Tasks.ToList();
        }
        
    }
}
