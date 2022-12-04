using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITaskCosmosService>(options =>
{
    string url = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("URL");
    string primaryKey = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("PrimaryKey");
    string dbName = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("DatabaseName");
    string containerName = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("ContainerName");

    var cosmosClient = new CosmosClient(
        url,
        primaryKey
    );
    return new TaskCosmosService(cosmosClient, dbName, containerName);
});
builder.Services.AddSingleton<IProjectCosmosService>(options =>
{
    string url = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("URL");
    string primaryKey = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("PrimaryKey");
    string dbName = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("DatabaseName");
    string containerName = builder.Configuration.GetSection("AzureCosmosDbSettings")
    .GetValue<string>("ContainerName");

    var cosmosClient = new CosmosClient(
        url,
        primaryKey
    );
    return new ProjectCosmosService(cosmosClient, dbName, containerName);
});

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
