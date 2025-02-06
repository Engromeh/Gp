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
      [MaxLength(200)]
      public string CourseDescription { get; set; }= null!;
      public string CourseRating { get; set; } = null!;

      [ForeignKey("Certificate")]
      public virtual int CertificateId { get; set; }
      public virtual Certificate Certificate { get; set; }=null!;
      public virtual ICollection<Video>Videos { get; set; }=new List<Video>();
      [ForeignKey("Admin")]
      public virtual int AdminId { get; set; }
      public virtual Admin Admin { get; set; } = null!;
      [ForeignKey("Instructor")]
      public virtual int InstructorId { get; set; }
      public virtual Instructor Instructor { get; set; } = null!;


   

    }
}
