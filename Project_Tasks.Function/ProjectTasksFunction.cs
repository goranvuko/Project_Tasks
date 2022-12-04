using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Project_Tasks.Function.Models;
namespace Project_Tasks.Function
{
    public class ProjectTasksFunction
    {
        private readonly IProjectTasksService projectTasksService;
        public ProjectTasksFunction(IProjectTasksService projectTasksService)
        {
            this.projectTasksService = projectTasksService;
        }

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
            string cosmosDbURL = config["CosmosApiURL"];
            try
            {
                log.LogInformation($"Synchronisation started at {DateTime.Now}");

                await this.projectTasksService.Sync(webApiBaseURL, cosmosDbURL, "Projects");

                log.LogInformation($"Synchronisation finished successfully at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                log.LogError($"Synchronisation failed at {DateTime.Now} with error: {ex}");
            }
        }
    }
}
