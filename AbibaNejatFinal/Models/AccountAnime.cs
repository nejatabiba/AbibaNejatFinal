using System.ComponentModel.DataAnnotations;

namespace AbibaNejatFinal.Models
{
    /// <summary>
    /// Join entity linking an Account to an Anime (by MAL ID) with user-specific data.
    /// </summary>
    public class AccountAnime
    {
        // Composite key: (AccountId, MalId) - configured in DbContext
        public int AccountId { get; set; }
        public int MalId { get; set; }


        [Range(1, 10)]
        public int? Rating { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RatedAt { get; set; }

        // Navigation property
        public Account? Account { get; set; }
    }
}