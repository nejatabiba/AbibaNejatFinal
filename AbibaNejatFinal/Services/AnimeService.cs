using AbibaNejatFinal.Models;
using System.Text.Json;
using System.Web;

namespace AbibaNejatFinal.Services
{
    public class AnimeService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "https://api.jikan.moe/v4";

        public AnimeService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Anime> GetAnimeAsync(int malId)
        {
            var response = await _client.GetStringAsync($"{BaseUrl}/anime/{malId}");
            var animeData = JsonSerializer.Deserialize<JikanAnimeResponse>(response);

            return MapToAnime(animeData.Data);
        }

        public async Task<AnimeBrowseViewModel> SearchAnimeAsync(string query = null, int page = 1)
        {
            var url = $"{BaseUrl}/anime?page={page}&order_by=score&sort=desc";

            if (!string.IsNullOrWhiteSpace(query))
            {
                url += $"&q={HttpUtility.UrlEncode(query)}";
            }

            var response = await _client.GetStringAsync(url);
            var result = JsonSerializer.Deserialize<JikanAnimeListResponse>(response);

            var viewModel = new AnimeBrowseViewModel
            {
                SearchQuery = query,
                CurrentPage = result.Pagination.CurrentPage,
                TotalPages = result.Pagination.LastVisiblePage,
                HasNextPage = result.Pagination.HasNextPage,
                TotalItems = result.Pagination.Items.Total,
                Animes = result.Data?.Select(MapToAnime).ToList() ?? new List<Anime>()
            };

            return viewModel;
        }

        private Anime MapToAnime(AnimeData data)
        {
            return new Anime
            {
                MalId = data.MalId,
                Title = data.TitleEnglish ?? data.Title,
                Summary = data.Synopsis,
                Episodes = data.Episodes,
                ImageUrl = data.Images?.Jpg?.ImageUrl,
                LargeImageUrl = data.Images?.Jpg?.LargeImageUrl,
                Score = data.Score
            };
        }
    }
}