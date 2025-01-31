using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Instructor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; }= null!;
        [Required]
        public string Email { get; set; }=null!;
        [Required]
        private string Password { get; set; }=null !;
        public virtual ICollection<Massage> Massages { get; set; } = new List<Massage>();
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
