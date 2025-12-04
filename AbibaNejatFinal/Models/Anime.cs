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

    // Pagination models for /anime endpoint
    public class JikanAnimeListResponse
    {
        [JsonPropertyName("pagination")]
        public JikanPagination Pagination { get; set; }

        [JsonPropertyName("data")]
        public List<AnimeData> Data { get; set; }
    }

    public class JikanPagination
    {
        [JsonPropertyName("last_visible_page")]
        public int LastVisiblePage { get; set; }

        [JsonPropertyName("has_next_page")]
        public bool HasNextPage { get; set; }

        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("items")]
        public JikanPaginationItems Items { get; set; }
    }

    public class JikanPaginationItems
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }
    }

    // ViewModel for Browse page
    public class AnimeBrowseViewModel
    {
        public List<Anime> Animes { get; set; } = new List<Anime>();
        public string SearchQuery { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public int TotalItems { get; set; }
    }
}