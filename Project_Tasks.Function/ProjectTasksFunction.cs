using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Project_Tasks.Function.Models;
namespace Project_Tasks.Function
{
    public class ProjectTasksFunction
    {
        [FunctionName("ProjectTasksFunction")]
        //0 0 * * * * , run every hour
        public async Task Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("host.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var webApiBaseURL = config["WebApiURL"];

            var webApiProjects =await GetAllProjectsFromApi(webApiBaseURL);
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
