using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }
        public Course Course { get; set; }
        
        [ForeignKey("CourseId")]
        public int? CourseId {  get; set; }
        public string? Title {  get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }


    }
}
