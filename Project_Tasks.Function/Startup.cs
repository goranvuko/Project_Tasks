using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Configuration;

[assembly: FunctionsStartup(typeof(Project_Tasks.Function.Startup))]

namespace Project_Tasks.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IProjectTasksService>(options =>
            {
                var section = builder.GetContext().Configuration.GetSection("AzureFunctionsJobHost:AzureCosmosDbSettings");
                string url = section.GetValue<string>("URL");
                string primaryKey = section
                .GetValue<string>("PrimaryKey");
                string dbName = section
                .GetValue<string>("DatabaseName");
                string containerName = section
                .GetValue<string>("ContainerName");

                var cosmosClient = new CosmosClient(
                    url,
                    primaryKey
                );
                return new ProjectTasksService(cosmosClient, dbName, containerName);
            });
        }
    }
}