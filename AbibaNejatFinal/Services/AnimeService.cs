using AbibaNejatFinal.Models;
using System.Text.Json;

namespace AbibaNejatFinal.Services
{
    public class AnimeService
    {
        private readonly HttpClient _client;

        public AnimeService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Anime> GetAnimeAsync(int malId)
        {
            var response = await _client.GetStringAsync($"https://api.jikan.moe/v4/anime/{malId}");
            var animeData = JsonSerializer.Deserialize<JikanAnimeResponse>(response);

            return new Anime
            {
                MalId = animeData.Data.MalId,
                Title = animeData.Data.TitleEnglish ?? animeData.Data.Title,
                Summary = animeData.Data.Synopsis,
                Episodes = animeData.Data.Episodes,
                ImageUrl = animeData.Data.Images.Jpg.ImageUrl,
                LargeImageUrl = animeData.Data.Images.Jpg.LargeImageUrl,
                Score = animeData.Data.Score
            };
        }
    }
}