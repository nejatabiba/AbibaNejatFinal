using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AbibaNejatFinal.Models
{
    public class Anime
    {
        [Key]
        public int Id { get; set; } // Primary key for your database
        public int MalId { get; set; } // MyAnimeList ID, useful for API updates

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Summary { get; set; }
        public int? Episodes { get; set; } // Nullable for unknown episodes
        public string ImageUrl { get; set; } // Main image
        public string LargeImageUrl { get; set; } // Large/high-quality image
        public double? Score { get; set; } // Nullable in case no score

        // Optional: link to MyAnimeList page
        public string MalUrl { get; set; }

        // Optional: user-specific data (not stored here, but in a join table)
    }

    public class JikanAnimeResponse
    {
        [JsonPropertyName("data")]
        public AnimeData Data { get; set; }
    }

    public class AnimeData
    {
        [JsonPropertyName("mal_id")]
        public int MalId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("title_english")]
        public string TitleEnglish { get; set; }

        [JsonPropertyName("synopsis")]
        public string Synopsis { get; set; }

        [JsonPropertyName("episodes")]
        public int? Episodes { get; set; }   // nullable because some anime have unknown episodes

        [JsonPropertyName("images")]
        public AnimeImages Images { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; }
    }

    public class AnimeImages
    {
        [JsonPropertyName("jpg")]
        public AnimeJpg Jpg { get; set; }

        [JsonPropertyName("webp")]
        public AnimeWebp Webp { get; set; }
    }

    public class AnimeJpg
    {
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("small_image_url")]
        public string SmallImageUrl { get; set; }

        [JsonPropertyName("large_image_url")]
        public string LargeImageUrl { get; set; }
    }

    public class AnimeWebp
    {
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("small_image_url")]
        public string SmallImageUrl { get; set; }

        [JsonPropertyName("large_image_url")]
        public string LargeImageUrl { get; set; }
    }
}