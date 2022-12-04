using Newtonsoft.Json;

namespace Project_Tasks.CosmosWebAPI.Models
{
    public class Task
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }
    }
}
