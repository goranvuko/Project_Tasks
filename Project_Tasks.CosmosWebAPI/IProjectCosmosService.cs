using Project_Tasks;

public interface IProjectCosmosService
{
    Task<IEnumerable<Project>> GetAllProjectsAsync();
}