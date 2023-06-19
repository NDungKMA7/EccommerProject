using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.DTO
{
    public class RegistraionModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 8 characters.")]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ComfirmPassword { get; set; }

        [Required]
        public string NumberPhone { get; set; }
        [Required]
        public string Address { get; set; } 
    }
}
