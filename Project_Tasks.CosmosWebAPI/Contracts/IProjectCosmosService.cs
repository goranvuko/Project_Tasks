using Project_Tasks.CosmosWebAPI.Models;

public interface IProjectCosmosService
{
    Task<IEnumerable<Project>> GetAllProjectsAsync();
}