using Microsoft.AspNetCore.Mvc;
using Project_Tasks.Data;
using Project_Tasks.Data.Entities;

namespace Project_Tasks.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectTasksRepository projectTasksRepository;

        public ProjectController(IProjectTasksRepository projectTasksRepository)
        {
            this.projectTasksRepository = projectTasksRepository;
        }

        [HttpGet(Name = "GetProjects")]
        public IEnumerable<Project> GetAll()
        {
           return projectTasksRepository.GetAllProjects();
        }
    }
}