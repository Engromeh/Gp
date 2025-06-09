using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace Learning_Academy.Models
{
    public class User :IdentityUser

    {
        
        public  string UserRole { get; set; }
        public virtual Student ? Student { get; set; }
        public virtual Instructor ? Instructor { get; set; }
        public virtual Admin ? Admin { get; set; }
        public virtual Profile ? Profile { get; set; }


    }
}
