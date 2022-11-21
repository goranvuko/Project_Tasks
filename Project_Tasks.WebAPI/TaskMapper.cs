using Project_Tasks.WebAPI.Models;

namespace Project_Tasks.WebAPI
{
    public class TaskMapper : ITaskMapper
    {
        public GetTaskDto MapToDto(Data.Entities.Task entity)
        {
            return new GetTaskDto { Id = entity.Id, Name = entity.Name, Descriptiom = entity.Description };
        }

        public Data.Entities.Task MapToEntity(AddTaskDto taskDto)
        {
            return new Data.Entities.Task { Name = taskDto.Name, Description = taskDto.Description, ProjectID = taskDto.ProjectID };
        }
    }
}
