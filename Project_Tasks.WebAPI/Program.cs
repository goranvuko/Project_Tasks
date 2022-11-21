using Microsoft.EntityFrameworkCore;
using Project_Tasks.Data;
using Microsoft.Extensions.Configuration;
using Project_Tasks.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProjectTasksDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));
builder.Services.AddTransient<IProjectRepository,ProjectRepository>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<IProjectMapper, ProjectMapper>();
builder.Services.AddTransient<ITaskMapper, TaskMapper>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
