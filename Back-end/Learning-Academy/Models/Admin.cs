using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }=null!;
        [Required]
        private string Password { get; set; }=null !;
        [ForeignKey(nameof(User))]
        public virtual string? UserId { get; set; }
        public virtual User ?User { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        


    }
}
