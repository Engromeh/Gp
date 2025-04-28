using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    
    public class CourseRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Range(1, 5)]
        public int RatingValue { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Student")]
        public virtual int StudentId { get; set; }
        public virtual Student Student { get; set; } = null!;
        [ForeignKey("Course")]
        public virtual int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;
    }
}
