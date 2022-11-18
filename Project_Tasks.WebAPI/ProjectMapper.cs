using Project_Tasks.WebAPI.Models;

namespace Project_Tasks.WebAPI
{
    public class ProjectMapper : IProjectMapper
    {
        public GetProjectDto MapToDto(Data.Entities.Project entity)
        {
            return new GetProjectDto { Id = entity.Id, Name = entity.Name, Code = entity.Code };
        }
    }
}
