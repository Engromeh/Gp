using System.ComponentModel.DataAnnotations;

namespace Learning_Academy.DTO
{
    public class ForgetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
