using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AbibaNejatFinal.Models
{
    /// <summary>
    /// Represents an anime entry (mapped from Jikan API data).
    /// </summary>
    public class Anime
    {

        public int MalId { get; set; }
        public string Title { get; set; }

        public string Summary { get; set; }
        public int? Episodes { get; set; }
        public string ImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public double? Score { get; set; }
    }

    #region Jikan API Response DTOs

    /// <summary>
    /// Response wrapper for single anime from Jikan API.
    /// </summary>
    public class JikanAnimeResponse
    {
        [JsonPropertyName("data")]
        public AnimeData Data { get; set; }
    }

    /// <summary>
    /// Anime data from Jikan API.
    /// </summary>
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
        public int? Episodes { get; set; }

        [JsonPropertyName("images")]
        public AnimeImages Images { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; }
    }

    public class AnimeImages
    {
        [JsonPropertyName("jpg")]
        public AnimeJpg Jpg { get; set; }
    }

    public class AnimeJpg
    {
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("large_image_url")]
        public string LargeImageUrl { get; set; }
    }

    /// <summary>
    /// Response wrapper for anime list from Jikan API.
    /// </summary>
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
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }
    }

    #endregion

    #region ViewModels

    /// <summary>
    /// ViewModel for the Browse page with pagination.
    /// </summary>
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

    /// <summary>
    /// ViewModel for MyList page - includes user's rating.
    /// </summary>
    public class UserAnimeViewModel
    {
        public Anime Anime { get; set; }
        public int? UserRating { get; set; }
        public DateTime AddedAt { get; set; }
    }

    #endregion
}