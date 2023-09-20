using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class CourseContent
    {
        [Key]
        public int Id { get; set; }
        public Course Course { get; set; }
        [ForeignKey("CourseId")]
        public int CourseId { get; set;}
        [Required]
        public string Title { get; set; }
        [Required]
        public int OrderNum {  get; set; }
        public string Content {  get; set; }
        public bool IsCompleted {  get; set; }




        
    }
}
