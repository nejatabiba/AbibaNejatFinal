using System.Security.Claims;
using AbibaNejatFinal.Data;
using AbibaNejatFinal.Models;
using AbibaNejatFinal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbibaNejatFinal.Controllers
{
    public class AnimeController : Controller
    {
        private readonly AnimeService _animeService;
        private readonly ApplicationDbContext _db;

        public AnimeController(AnimeService animeService, ApplicationDbContext db)
        {
            _animeService = animeService;
            _db = db;
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

                // Check if anime is in user's list and get their rating
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                    var accountAnime = await _db.AccountAnimes
                        .FirstOrDefaultAsync(aa => aa.AccountId == userId && aa.MalId == id);
                    ViewBag.IsInList = accountAnime != null;
                    ViewBag.UserRating = accountAnime?.Rating;
                }

                return View(anime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching anime {id}: {ex.Message}");
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToList(int malId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var exists = await _db.AccountAnimes.AnyAsync(aa => aa.AccountId == userId && aa.MalId == malId);
            if (!exists)
            {
                var accountAnime = new AccountAnime
                {
                    AccountId = userId,
                    MalId = malId,
                    AddedAt = DateTime.UtcNow
                };
                _db.AccountAnimes.Add(accountAnime);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Anime added to your list!";
            }

            return RedirectToAction("Details", new { id = malId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromList(int malId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var accountAnime = await _db.AccountAnimes
                .FirstOrDefaultAsync(aa => aa.AccountId == userId && aa.MalId == malId);

            if (accountAnime != null)
            {
                _db.AccountAnimes.Remove(accountAnime);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Anime removed from your list.";
            }

            return RedirectToAction("Details", new { id = malId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateAnime(int malId, int rating)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var accountAnime = await _db.AccountAnimes
                .FirstOrDefaultAsync(aa => aa.AccountId == userId && aa.MalId == malId);

            if (accountAnime != null)
            {
                accountAnime.Rating = rating;
                accountAnime.RatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Rated {rating}/10!";
            }

            return RedirectToAction("Details", new { id = malId });
        }

        [Authorize]
        public async Task<IActionResult> MyList()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var userAnimes = await _db.AccountAnimes
                .Where(aa => aa.AccountId == userId)
                .OrderByDescending(aa => aa.AddedAt)
                .ToListAsync();

            var animeList = new List<UserAnimeViewModel>();
            foreach (var aa in userAnimes)
            {
                try
                {
                    var anime = await _animeService.GetAnimeAsync(aa.MalId);
                    animeList.Add(new UserAnimeViewModel
                    {
                        Anime = anime,
                        UserRating = aa.Rating,
                        AddedAt = aa.AddedAt
                    });
                    // Respect API rate limits
                    await Task.Delay(350);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching anime {aa.MalId}: {ex.Message}");
                }
            }

            return View(animeList);
        }
    }
}
