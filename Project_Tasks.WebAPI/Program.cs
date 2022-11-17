using Microsoft.EntityFrameworkCore;
using Project_Tasks.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProjectTasksDbContext>(opt =>
    opt.UseSqlServer("Data Source=DESKTOP-0UCKG6H\\SQLEXPRESS;Initial Catalog=ProjectTasksDb;Trusted_Connection=True;Integrated Security=True;Trust Server Certificate=True;"));
builder.Services.AddTransient<IProjectTasksRepository, ProjectTasksRepository>();
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
