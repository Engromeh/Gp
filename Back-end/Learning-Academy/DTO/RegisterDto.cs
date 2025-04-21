using System.ComponentModel.DataAnnotations;

namespace Learning_Academy.DTO
{
    public class RegisterDto
    {
        [Required]
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public required string Phone {  get; set; }
        [Required]
       // [RegularExpression("^(Female|Male) $",ErrorMessage ="Gender must be male or female")]
        public  required string Gender { get; set; }
        [Required]
        //[RegularExpression("^(Admin|Student|Instructor) $", ErrorMessage = "role must be Admin , Instructor or Student ")]
        public required string Role { get; set; }

    }
}
