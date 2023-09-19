using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public User User { get; set; }
        [ForeignKey("Id")]
        public int? InstructorId { get; set; }
        public string? Category {  get; set; }
        public int? EnrollmentCount {  get; set; }
        public string? ImageUrl {  get; set; }

    }
}
