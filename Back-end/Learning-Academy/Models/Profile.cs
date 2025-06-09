using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        [ForeignKey("User")]
        public virtual string ? UserId { get; set; }
        public virtual User ? User { get; set; }
    }
}
