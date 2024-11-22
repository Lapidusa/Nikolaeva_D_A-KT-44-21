using System.Text.Json.Serialization;

namespace project.Models
{
    public class Curriculum
    {   
        public int CurriculumId { get; set; }
        public int GroupId { get; set; }
        // [JsonIgnore]
        public Group Group { get; set; }
        public int ObjectId { get; set; }
        // [JsonIgnore]
        public Objects Objects { get; set; }
        public int Hours {get; set;}
    }
}