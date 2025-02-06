using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; }= null!;
        [Required]
        public string Email { get; set; } = null !;
        public string Country { get; set; }
        [ForeignKey("Admin")]
        public virtual int AdminId { get; set; }
        public virtual Admin Admin { get; set; } = null!;
        public virtual ICollection<StudentEnrollmentCourse> StudentEnrollments { get; set; }=new List<StudentEnrollmentCourse>();
        public virtual ICollection<StudentRatingCourse>Rates { get; set; }=new List<StudentRatingCourse>();
        public virtual ICollection<Massage> Massages { get; set; } = new List<Massage>();
       
    } 
}
