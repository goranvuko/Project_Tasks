using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Project_Tasks.CosmosWebAPI.Models;

namespace Project_Tasks.CosmosWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskCosmosService taskCosmosService;

        public TasksController(ITaskCosmosService taskCosmosService)
        {
            this.taskCosmosService = taskCosmosService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks =await taskCosmosService.GetAllTasksAsync();
            return Ok(tasks.Select(t=> new TaskDTO
            {
                Id = t.Id,  
                Name = t.Name,
                Description = t.Description,
            }));
        }
    }
}