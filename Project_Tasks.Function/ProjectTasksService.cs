using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Project_Tasks.Function.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;

namespace Project_Tasks.Function
{
    public class ProjectTasksService : IProjectTasksService
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public ProjectTasksService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
        public async Task Sync(string webApiURL, string cosmosDbURL, string route)
        {
            var webApiProjects = await GetAllProjectsFromApi(webApiURL);
            var cosmosProjects = await GetAllProjectsFromApi(cosmosDbURL);

            var webApiProjectIds = webApiProjects.Select(t=> t.Id).ToList();
            var cosmosProjectsIds = cosmosProjects.Select(t => t.Id).ToList();
            var idsToAdd = webApiProjectIds.Except(cosmosProjectsIds);
            var idsToDelete = webApiProjectIds.Except(cosmosProjectsIds);
            var idsToUpdate = webApiProjectIds.Intersect(cosmosProjectsIds);

            var entitiesToAdd = webApiProjects.Where(t => idsToAdd.Contains(t.Id));
            await AddNewProjects(entitiesToAdd);


            var entitiesToUpdate = webApiProjects.Where(t => idsToUpdate.Contains(t.Id));
            await UpdateExistingProjects(entitiesToUpdate);

            var entitiesToDelete = webApiProjects.Where(t => idsToDelete.Contains(t.Id));
            await DeleteExistingProjects(entitiesToDelete);

        }
        private async Task AddNewProjects(IEnumerable<GetProjectDto> projects)
        {
            foreach (var entity in projects)
            {
                try
                {
                    var dto = new AddProjectDto { Id = entity.Id.ToString(), Code = entity.Code, Name = entity.Name };
                    var response = await _container.CreateItemAsync(dto, new Microsoft.Azure.Cosmos.PartitionKey(dto.Id));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to add project with id: {entity.Id}");
                }
            }
        }
        private async Task UpdateExistingProjects(IEnumerable<GetProjectDto> projects)
        {
            foreach (var entity in projects)
            {
                try
                {
                    ItemResponse<GetProjectDto> res = await _container.ReadItemAsync<GetProjectDto>(entity.Id.ToString(), new Microsoft.Azure.Cosmos.PartitionKey(entity.Id));
                    //Get Existing Item
                    var existingItem = res.Resource;
                    //Replace existing item values with new values
                    existingItem.Name = entity.Name;
                    existingItem.Code = entity.Code;
                    
                    var updateRes = await _container.ReplaceItemAsync(existingItem, entity.Id.ToString(), new Microsoft.Azure.Cosmos.PartitionKey(entity.Id));
                    
                    //ItemResponse<GetProjectDto> itemResponse = await _container.ReadItemAsync<GetProjectDto>(entity.Id.ToString(), new PartitionKey(entity.Id));
                    //var itemBody = itemResponse.Resource;
                    //itemBody.Name= entity.Name;
                    //itemBody.Id=entity.Id;
                    //itemBody.Code = entity.Code;
                    //var response = await _container.ReplaceItemAsync(itemBody,entity.Id.ToString(), new PartitionKey(entity.Id));

                    //IReadOnlyList<PatchOperation> patchOperations = new List<PatchOperation>
                    //{
                    //    PatchOperation.Replace("/name", entity.Name),
                    //    PatchOperation.Replace("/code", entity.Code)
                    //};
                    //var response = await _container.PatchItemAsync<GetProjectDto>(entity.Id.ToString(), new PartitionKey(entity.Id),patchOperations);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to update project with id: {entity.Id}");
                }
            }
        }
        private async Task DeleteExistingProjects(IEnumerable<GetProjectDto> projects)
        {
            foreach (var entity in projects)
            {
                try
                {
                    var response = await _container.DeleteItemAsync<GetProjectDto>(entity.Id.ToString(), new Microsoft.Azure.Cosmos.PartitionKey(entity.Id));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete project with id: {entity.Id}");
                }
            }
        }

        private async Task<List<GetProjectDto>> GetAllProjectsFromApi(string webApiBaseURL)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(webApiBaseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync("projects");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<GetProjectDto>>();
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                    return await Task.FromResult<List<GetProjectDto>>(Enumerable.Empty<GetProjectDto>().ToList());
                }
            }
        }
    }
}
