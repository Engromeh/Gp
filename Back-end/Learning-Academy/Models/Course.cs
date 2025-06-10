using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string CourseName { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string CourseDescription { get; set; } = null!;
       
        [Required]
        public string? ImagePath { get; set; }

        [Required]
        public string Category { get; set; } = null!;


        [ForeignKey("Instructor")]
        public virtual int? InstructorId { get; set; }
        public virtual Instructor? Instructor { get; set; } = null!;

        public virtual ICollection<Level> Levels { get; set; } = new List<Level>();
        public virtual ICollection<CourseRating> CourseRatinds { get; set; } 



    }
}