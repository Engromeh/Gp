using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Option
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string text { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
        [ForeignKey("Qustion")]
        public int? QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
