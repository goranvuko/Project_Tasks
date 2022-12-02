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

namespace Project_Tasks.Function
{
    public class ProjectTasksService : IProjectTasksService
    {
        private readonly Container _container;

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

            var entitiesToAdd = webApiProjects.Where(t => !cosmosProjects.Contains(t));
            var entitiesToDelete = cosmosProjects.Where(t => !webApiProjects.Contains(t));
        }
        private async Task AddNewProjects(List<GetProjectDto> projects)
        {
            
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
