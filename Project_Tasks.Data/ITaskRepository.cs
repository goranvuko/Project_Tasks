namespace Project_Tasks.Data
{
    public interface ITaskRepository
    {
        void AddTask(Entities.Task task);
        void DeleteTask(int id);
        IEnumerable<Entities.Task> GetAllTasks();
        Entities.Task GetTask(int id);
    }
}