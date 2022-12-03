using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Project_Tasks;

public class ProjectCosmosService : IProjectCosmosService
{

    private readonly Container _container;

    public ProjectCosmosService(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
    }
    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        var queryable = _container.GetItemLinqQueryable<Project>();
        using FeedIterator<Project> feed = queryable
        .OrderByDescending(p => p.Id)
        .ToFeedIterator();

        List<Project> results = new();

        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            foreach (Project item in response)
            {
                results.Add(item);
            }
        }
        return results;
    }

}