using AbibaNejatFinal.Models;
using AbibaNejatFinal.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbibaNejatFinal.Controllers
{
    /// <summary>
    /// Controller for home and general pages.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly AnimeService _animeService;

        public HomeController(AnimeService animeService)
        {
            _animeService = animeService;
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            // Popular anime MAL IDs for the background slideshow
            var popularAnimeMalIds = new List<int>
            {
                38000,  // Demon Slayer
                1,      // Cowboy Bebop
                2001,   // Gurren Lagann
                11757,  // Sword Art Online
                31964,  // My Hero Academia
                38524,  // Attack on Titan Season 3 Part 2
                205,    // Samurai Champloo
                38680,  // Fruits Basket
                23755,  // The Seven Deadly Sins
                28249,  // The Heroic Legend of Arslan
                33,     // Berserk
                32281,  // Your Name.
                28851,  // A Silent Voice
                30,     // Neon Genesis Evangelion
                14719   // JoJo's Bizarre Adventure
            };

            // Randomly select 10 anime for variety
            var random = new Random();
            var selectedIds = popularAnimeMalIds
                .OrderBy(_ => random.Next())
                .Take(10)
                .ToList();

            var animeList = new List<Anime>();
            foreach (var malId in selectedIds)
            {
                try
                {
                    var anime = await _animeService.GetAnimeAsync(malId);
                    animeList.Add(anime);

                    // Respect Jikan API rate limits (3 requests/second)
                    await Task.Delay(400);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching anime {malId}: {ex.Message}");
                }
            }

            return View(animeList);
        }

        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }
    }
}