namespace project.Models
{
    public class Objects
    {
        public int ObjectId { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }

    }
}
