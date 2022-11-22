namespace Project_Tasks.Data
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task<Entities.Task> AddTask(Entities.Task task);
        System.Threading.Tasks.Task DeleteTask(int id);
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>>  GetAllTasks();
        System.Threading.Tasks.Task<Entities.Task> GetTaskAsync(int id);
    }
}