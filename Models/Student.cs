using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Student
    {
        [Key]   
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }

    }
}
