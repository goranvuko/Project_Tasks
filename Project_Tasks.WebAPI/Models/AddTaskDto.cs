namespace Project_Tasks.WebAPI.Models
{
    public class AddTaskDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int ProjectID { get; set; }
    }
}
