using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Massage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        [ForeignKey("Student")]
        public virtual int StudentId { get; set; }
        public virtual Student Student { get; set; }
        [ForeignKey(("Instructor"))]
        public virtual int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }=new List<Attachment>();
    }
}
