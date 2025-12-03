using AbibaNejatFinal.Models;
using AbibaNejatFinal.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbibaNejatFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly AnimeService _animeService;

        public HomeController(AnimeService animeService)
        {
            _animeService = animeService;
        }

        public async Task<IActionResult> Index()
        {
            // List of popular anime MAL IDs to display in background
            var popularAnimeMalIds = new List<int>
            {
                5114,  // Fullmetal Alchemist: Brotherhood
                16498, // Attack on Titan
                11061, // Hunter x Hunter
                9253,  // Steins;Gate
                28977, // Gintama
                38524, // Attack on Titan Season 3 Part 2
                9969,  // Gintama'
                820,   // Ginga Eiyuu Densetsu
                15417, // Gintama': Enchousen
                918    // Gintama
            };

            var animeList = new List<Anime>();

            foreach (var malId in popularAnimeMalIds.Take(6)) // Take 6 for rotation
            {
                try
                {
                    var anime = await _animeService.GetAnimeAsync(malId);
                    animeList.Add(anime);

                    // Add delay to respect Jikan API rate limits (3 requests/second)
                    await Task.Delay(400);
                }
                catch (Exception ex)
                {
                    // Log error but continue with other anime
                    Console.WriteLine($"Error fetching anime {malId}: {ex.Message}");
                }
            }

            return View(animeList);
        }
    }
}