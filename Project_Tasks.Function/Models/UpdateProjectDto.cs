using Newtonsoft.Json;

namespace Project_Tasks.Function.Models
{
    public class UpdateProjectDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
