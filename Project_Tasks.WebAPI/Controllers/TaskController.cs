using Microsoft.AspNetCore.Mvc;
using Project_Tasks.Data;
using Project_Tasks.WebAPI.Models;

namespace Project_Tasks.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository taskRepository;
        private readonly ITaskMapper taskMapper;

        public TaskController(ITaskRepository taskRepository, ITaskMapper taskMapper)
        {
            this.taskRepository = taskRepository;
            this.taskMapper = taskMapper;
        }
        [HttpGet(Name = "GetTasks")]
        public IActionResult GetAll()
        {
            var tasks = taskRepository.GetAllTasks();
            return Ok(tasks.Select(this.taskMapper.MapToDto));
        }
        [HttpPost(Name = "AddTask")]
        public IActionResult Add(AddTaskDto taskDto)
        {
            var task = taskMapper.MapToEntity(taskDto);
            var created = taskRepository.AddTask(task);
            return Ok(this.taskMapper.MapToDto(created));
        }
        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            Data.Entities.Task task = taskRepository.GetTask(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(this.taskMapper.MapToDto(task));
        }
    }
}
