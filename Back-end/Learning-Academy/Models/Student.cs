using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        
        [ForeignKey("Admin")]
        public virtual int? AdminId { get; set; }
        [JsonIgnore]
        public virtual Admin? Admin { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public virtual string? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<StudentEnrollmentCourse> StudentEnrollments { get; set; }=new List<StudentEnrollmentCourse>();
        public virtual ICollection<CourseRating>Rates { get; set; }=new List<CourseRating>();
        public virtual ICollection<Massage> Massages { get; set; } = new List<Massage>();
       
    } 
}
