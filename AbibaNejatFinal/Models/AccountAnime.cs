using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbibaNejatFinal.Models
{
    // Join entity between Account and external Anime ID (MalId)
    public class AccountAnime
    {
        // Composite key configured in DbContext.OnModelCreating
        public int AccountId { get; set; }

        // External anime identifier (MyAnimeList ID) used by your API service
        public int MalId { get; set; }

        // If the user wants to watch this anime (watchlist)
        public bool WantToWatch { get; set; } = false;

        // Nullable rating (1-10). Use int? so unrated entries are possible.
        [Range(1, 10)]
        public int? Rating { get; set; }

        // Optional timestamps
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RatedAt { get; set; }

        // Navigation to Account only (no local Anime navigation)
        public Account? Account { get; set; }
    }
}