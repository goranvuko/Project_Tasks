using Microsoft.EntityFrameworkCore;
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
        public async System.Threading.Tasks.Task<Entities.Task> AddTask(Entities.Task task)
        {
            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();
            return task;
        }
        public async System.Threading.Tasks.Task<Entities.Task> GetTaskAsync(int id)
        {
            return await dbContext.Tasks.SingleOrDefaultAsync(t=> t.Id == id);
        }

        public async System.Threading.Tasks.Task DeleteTask(int id)
        {
            var task = new Entities.Task { Id = id };
            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();
        }
        public async System.Threading.Tasks.Task<IEnumerable<Entities.Task>>  GetAllTasks()
        {
            return await dbContext.Tasks.ToListAsync();
        }
        
    }
}
