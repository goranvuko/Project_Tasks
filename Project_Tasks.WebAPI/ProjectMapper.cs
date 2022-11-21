using Project_Tasks.Data.Entities;
using Project_Tasks.WebAPI.Models;

namespace Project_Tasks.WebAPI
{
    public class ProjectMapper : IProjectMapper
    {
        public GetProjectDto MapToDto(Data.Entities.Project entity)
        {
            return new GetProjectDto { Id = entity.Id, Name = entity.Name, Code = entity.Code };
        }

        public Project MapToEntity(AddProjectDto projectDto)
        {
            return new Project { Code= projectDto.Code, Name= projectDto.Name };   
        }
    }
}
