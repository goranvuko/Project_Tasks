using Project_Tasks.WebAPI.Models;

namespace Project_Tasks.WebAPI
{
    public interface ITaskMapper
    {
        GetTaskDto MapToDto(Data.Entities.Task dto);
        Data.Entities.Task MapToEntity(AddTaskDto taskDto);
    }
}