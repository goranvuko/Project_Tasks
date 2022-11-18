using Project_Tasks.Data.Entities;
using Project_Tasks.WebAPI.Models;

namespace Project_Tasks.WebAPI
{
    public interface IProjectMapper
    {
        GetProjectDto MapToDto(Project entity);
    }
}