using Microsoft.AspNetCore.Mvc;
using Project_Tasks.Data;
using Project_Tasks.Data.Entities;
using Project_Tasks.WebAPI.Models;

namespace Project_Tasks.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository projectRepository;
        private readonly IProjectMapper projectMapper;

        public ProjectController(IProjectRepository projectTasksRepository, IProjectMapper projectMapper)
        {
            this.projectRepository = projectTasksRepository;
            this.projectMapper = projectMapper;
        }

        [HttpGet(Name = "GetProjects")]
        public IActionResult GetAll()
        {
            var projects = projectRepository.GetAllProjects();
           return Ok(projects.Select(p => this.projectMapper.MapToDto(p)));
        }
        [HttpPost(Name ="AddProject")]
        public IActionResult Add(AddProjectDto projectDto)
        {

            projectRepository.AddProject(project);
            return Ok(project);
        }
        [HttpGet("{id}")]
        //[Route("{id}")]
        public IActionResult GetProjectById(int id)
        {
            Project project = projectRepository.GetProject(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(this.projectMapper.MapToDto(project));
        }

    }
}