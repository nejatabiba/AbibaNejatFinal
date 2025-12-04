using AbibaNejatFinal.Models;
using AbibaNejatFinal.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbibaNejatFinal.Controllers
{
    public class AnimeController : Controller
    {
        private readonly AnimeService _animeService;

        public AnimeController(AnimeService animeService)
        {
            _animeService = animeService;
        }

        public async Task<IActionResult> Browse(string q, int page = 1)
        {
            try
            {
                var viewModel = await _animeService.SearchAnimeAsync(q, page);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching anime: {ex.Message}");
                return View(new AnimeBrowseViewModel());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var anime = await _animeService.GetAnimeAsync(id);
                return View(anime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching anime {id}: {ex.Message}");
                return NotFound();
            }
        }
    }
}
