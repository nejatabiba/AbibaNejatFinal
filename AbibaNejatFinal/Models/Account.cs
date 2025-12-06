using System.ComponentModel.DataAnnotations;

namespace AbibaNejatFinal.Models
{
    /// <summary>
    /// Represents a user account in the system.
    /// </summary>
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string? PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string Role { get; set; } = "User";

        // Navigation property
        public ICollection<AccountAnime> AccountAnimes { get; set; } = new List<AccountAnime>();
    }
}