using DurableTask.Core.Common;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Project_Tasks;
using Project_Tasks.CosmosWebAPI;

public class TaskCosmosService : ITaskCosmosService
{

    private readonly Container _container;

    public TaskCosmosService(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
    }
    public async Task<IEnumerable<Project_Tasks.CosmosWebAPI.Models.Task>> GetAllTasksAsync()
    {
        var queryable = _container.GetItemLinqQueryable<Project_Tasks.CosmosWebAPI.Models.Task>();
        using FeedIterator<Project_Tasks.CosmosWebAPI.Models.Task> feed = queryable
        .OrderByDescending(p => p.Id)
        .ToFeedIterator();

        List<Project_Tasks.CosmosWebAPI.Models.Task> results = new();

        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            foreach (Project_Tasks.CosmosWebAPI.Models.Task item in response)
            {
                results.Add(item);
            }
        }
        return results;
    }

}