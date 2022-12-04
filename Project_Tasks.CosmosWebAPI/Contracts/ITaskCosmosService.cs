public interface ITaskCosmosService
{
    Task<IEnumerable<Project_Tasks.CosmosWebAPI.Models.Task>> GetAllTasksAsync();
}