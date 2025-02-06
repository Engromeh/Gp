using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; }=null!;
        
        public int Size { get; set; }
        [ForeignKey("Massage")]
        public virtual int MassageId { get; set; }
        public virtual Massage Massage { get; set; } 


    }
}
