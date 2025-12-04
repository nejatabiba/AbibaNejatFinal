using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AbibaNejatFinal.Models
{
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

        // Store a securely hashed password (never plain text)
        [Required]
        [MaxLength(200)]
        public string? PasswordHash { get; set; }

        // Optional per-user salt (if your hashing strategy uses one)
        // Made nullable because the current sign-up flow uses PasswordHasher which does not require a separate salt.
        [MaxLength(200)]
        public string? PasswordSalt { get; set; }

        // Auditing / status
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Simple role string for authorization (expand to roles table if needed)
        [MaxLength(50)]
        public string Role { get; set; } = "User";

        // Concurrency token
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Navigation: join entities linking this account to Anime with extra data
        public ICollection<AccountAnime> AccountAnimes { get; set; } = new List<AccountAnime>();
    }
}