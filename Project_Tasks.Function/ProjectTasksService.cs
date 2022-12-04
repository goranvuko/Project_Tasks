using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Project_Tasks.Function.Models;
using Microsoft.Azure.Cosmos;

namespace Project_Tasks.Function
{
    public class ProjectTasksService : IProjectTasksService
    {
        private readonly Container projectContainer;
        private readonly Container taskContainer;

        public ProjectTasksService(CosmosClient cosmosClient, string databaseName, string projectContainerName,string taskContainerName)
        {
            projectContainer = cosmosClient.GetContainer(databaseName, projectContainerName);
            taskContainer = cosmosClient.GetContainer(databaseName, taskContainerName);
        }
        public async Task Sync(string webApiURL, string cosmosDbURL)
        {
            await SyncProjects(webApiURL, cosmosDbURL);
            await SyncTasks(webApiURL, cosmosDbURL);
        }

        private async Task SyncProjects(string webApiURL, string cosmosDbURL)
        {
            var webApiProjects = await GetAllEntitiesFromApi<GetProjectDto>(webApiURL, "projects");
            var cosmosProjects = await GetAllEntitiesFromApi<GetProjectDto>(cosmosDbURL, "projects");

            var webApiProjectIds = webApiProjects.Select(t => t.Id).ToList();
            var cosmosProjectsIds = cosmosProjects.Select(t => t.Id).ToList();
            var idsToAdd = webApiProjectIds.Except(cosmosProjectsIds);
            var idsToDelete = cosmosProjectsIds.Except(webApiProjectIds);
            var idsToUpdate = webApiProjectIds.Intersect(cosmosProjectsIds);

            var entitiesToAdd = webApiProjects.Where(t => idsToAdd.Contains(t.Id));
            await AddNewProjects(entitiesToAdd);

            var entitiesToUpdate = webApiProjects.Where(t => idsToUpdate.Contains(t.Id));
            await UpdateExistingProjects(entitiesToUpdate);

            var entitiesToDelete = cosmosProjects.Where(t => idsToDelete.Contains(t.Id));
            await DeleteExistingEntities(entitiesToDelete);
        }
        private async Task SyncTasks(string webApiURL, string cosmosDbURL)
        {
            var webApiTasks = await GetAllEntitiesFromApi<GetTaskDto>(webApiURL, "tasks");
            var cosmosTasks = await GetAllEntitiesFromApi<GetTaskDto>(cosmosDbURL, "tasks");

            var webApiTaskIds = webApiTasks.Select(t => t.Id).ToList();
            var cosmosTaskIds = cosmosTasks.Select(t => t.Id).ToList();
            var idsToAdd = webApiTaskIds.Except(cosmosTaskIds);
            var idsToDelete = cosmosTaskIds.Except(webApiTaskIds);
            var idsToUpdate = webApiTaskIds.Intersect(cosmosTaskIds);

            var entitiesToAdd = webApiTasks.Where(t => idsToAdd.Contains(t.Id));
            await AddNewTasks(entitiesToAdd);

            var entitiesToUpdate = webApiTasks.Where(t => idsToUpdate.Contains(t.Id));
            await UpdateExistingTasks(entitiesToUpdate);

            var entitiesToDelete = cosmosTasks.Where(t => idsToDelete.Contains(t.Id));
            await DeleteExistingEntities(entitiesToDelete);
        }

        private async Task AddNewProjects(IEnumerable<GetProjectDto> projects)
        {
            foreach (var entity in projects)
            {
                try
                {
                    var dto = new AddProjectDto { Id = entity.Id.ToString(), Code = entity.Code, Name = entity.Name };
                    var stringId = entity.Id.ToString();
                    var response = await projectContainer.CreateItemAsync(dto, new PartitionKey(stringId));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to add project with id: {entity.Id}");
                }
            }
        }
        private async Task AddNewTasks(IEnumerable<GetTaskDto> tasks)
        {
            foreach (var entity in tasks)
            {
                try
                {
                    var dto = new AddTaskDto { Id = entity.Id.ToString(),ProjectId=entity.ProjectId, Description = entity.Description, Name = entity.Name };
                    var stringId = entity.Id.ToString();
                    var response = await taskContainer.CreateItemAsync(dto, new PartitionKey(stringId));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to add task with id: {entity.Id}");
                }
            }
        }
        private async Task UpdateExistingProjects(IEnumerable<GetProjectDto> projects)
        {
            foreach (var entity in projects)
            {
                try
                {
                    IReadOnlyList<PatchOperation> patchOperations = new List<PatchOperation>
                    {
                        PatchOperation.Replace("/name", entity.Name),
                        PatchOperation.Replace("/code", entity.Code)
                    };

                    var stringId = entity.Id.ToString();
                    var response = await projectContainer.PatchItemAsync<GetProjectDto>(stringId, new PartitionKey(stringId), patchOperations);

                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to update project with id: {entity.Id}");
                }
            }
        }
        private async Task UpdateExistingTasks(IEnumerable<GetTaskDto> tasks)
        {
            foreach (var entity in tasks)
            {
                try
                {
                    IReadOnlyList<PatchOperation> patchOperations = new List<PatchOperation>
                    {
                        PatchOperation.Replace("/name", entity.Name),
                        PatchOperation.Replace("/description", entity.Description),
                        PatchOperation.Replace("/project_id", entity.ProjectId)
                    };

                    var stringId = entity.Id.ToString();
                    var response = await taskContainer.PatchItemAsync<GetTaskDto>(stringId, new PartitionKey(stringId), patchOperations);

                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to update task with id: {entity.Id}");
                }
            }
        }
       
        private async Task DeleteExistingEntities<T>(IEnumerable<T> entities) where T : Entity
        {
            foreach (var entity in entities)
            {
                try
                {
                    var stringId = entity.Id.ToString();
                    var response = await taskContainer.DeleteItemAsync<T>(stringId, new PartitionKey(stringId));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete {nameof(T)} with id: {entity.Id}");
                }
            }
        }

        private async Task<List<T>> GetAllEntitiesFromApi<T>(string webApiBaseURL, string route)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(webApiBaseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<T>>();
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                    return await Task.FromResult(Enumerable.Empty<T>().ToList());
                }
            }
        }
    }
}
