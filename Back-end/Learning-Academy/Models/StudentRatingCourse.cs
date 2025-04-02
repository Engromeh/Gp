using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    [PrimaryKey("StudentId", "CourseId")]
    public class StudentRatingCourse
    {
        public int Rating { get; set; } 

        [ForeignKey("Student")]
       
        public virtual int StudentId { get; set; }
        public virtual Student Student { get; set; } = null!;
        [ForeignKey("Course")]
        public virtual int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;
    }
}
