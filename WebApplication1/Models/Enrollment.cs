using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public int? UserId {  get; set; }
        public Course Course { get; set; }
        [ForeignKey("Course")]
        public int? CourseId {  get; set; }
        public DateTime? EnrollmentDate { get; set; }

    }
}
