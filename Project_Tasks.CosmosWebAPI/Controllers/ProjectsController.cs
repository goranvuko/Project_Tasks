using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Project_Tasks.CosmosWebAPI.Models;

namespace Project_Tasks.CosmosWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsCotnroller : ControllerBase
    {
        private readonly IProjectCosmosService projectCosmosService;

        public ProjectsCotnroller(IProjectCosmosService projectCosmosService)
        {
            this.projectCosmosService = projectCosmosService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects =await projectCosmosService.GetAllProjectsAsync();
            return Ok(projects.Select(t=> new ProjectDTO
            {
                Id = t.Id,  
                Name = t.Name,
                Code = t.Code,
            }));
        }
    }
}