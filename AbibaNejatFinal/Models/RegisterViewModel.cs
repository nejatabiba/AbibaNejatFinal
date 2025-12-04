using System.ComponentModel.DataAnnotations;

namespace AbibaNejatFinal.Models
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}