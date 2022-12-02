using System.Threading.Tasks;

namespace Project_Tasks.Function
{
    public interface IProjectTasksService
    {
        Task Sync(string webApiURL, string cosmosDbURL, string route);
    }
}