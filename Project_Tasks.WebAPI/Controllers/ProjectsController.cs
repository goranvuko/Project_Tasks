using Microsoft.AspNetCore.Mvc;
using Project_Tasks.Data;
using Project_Tasks.Data.Entities;
using Project_Tasks.WebAPI.Models;

namespace Project_Tasks.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository projectRepository;
        private readonly IProjectMapper projectMapper;

        public ProjectsController(IProjectRepository projectTasksRepository, IProjectMapper projectMapper)
        {
            this.projectRepository = projectTasksRepository;
            this.projectMapper = projectMapper;
        }

        [HttpGet(Name = "GetProjects")]
        public IActionResult GetAll()
        {
            var projects = projectRepository.GetAllProjects();
            return Ok(projects.Select(this.projectMapper.MapToDto));
        }
        [HttpPost(Name ="AddProject")]
        public IActionResult Add(AddProjectDto projectDto)
        {
            var project = this.projectMapper.MapToEntity(projectDto);
            projectRepository.AddProject(project);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, this.projectMapper.MapToDto(project));
        }
        [HttpGet("{id}")]
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