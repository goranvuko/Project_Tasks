using Project_Tasks.Data.Entities;

namespace Project_Tasks.Data
{
    public interface IProjectTasksRepository
    {
        void AddProject(Project project);
        IEnumerable<Project> GetAllProjects();
        Project GetProject(int id);
    }
}