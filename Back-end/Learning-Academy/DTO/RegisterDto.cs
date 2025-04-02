using System.ComponentModel.DataAnnotations;

namespace Learning_Academy.DTO
{
    public class RegisterDto
    {
        
        public required string UserName { get; set; }
        public required string Password { get; set; }
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
        public required string Email { get; set; }
        public required string Phone {  get; set; }
        public  required string Gender { get; set; }
        public required string Role { get; set; }

    }
}
