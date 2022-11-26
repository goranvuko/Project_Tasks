using Microsoft.AspNetCore.Mvc;
using Project_Tasks.Data;
using Project_Tasks.WebAPI.Models;

namespace Project_Tasks.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository taskRepository;
        private readonly ITaskMapper taskMapper;

        public TasksController(ITaskRepository taskRepository, ITaskMapper taskMapper)
        {
            this.taskRepository = taskRepository;
            this.taskMapper = taskMapper;
        }
        [HttpGet(Name = "GetTasks")]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await taskRepository.GetAllTasks();
            return Ok(tasks.Select(this.taskMapper.MapToDto));
        }
        [HttpPost(Name = "AddTask")]
        public async Task<IActionResult> Add(AddTaskDto taskDto)
        {
            var task = taskMapper.MapToEntity(taskDto);
            var created = await taskRepository.AddTask(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = created.Id }, this.taskMapper.MapToDto(created));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            Data.Entities.Task task = await taskRepository.GetTaskAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(this.taskMapper.MapToDto(task));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this.taskRepository.DeleteTask(id);
            return NoContent();
        }
    }
}
