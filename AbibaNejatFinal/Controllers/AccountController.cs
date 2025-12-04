using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using AbibaNejatFinal.Data;
using AbibaNejatFinal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbibaNejatFinal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<Account> _passwordHasher;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<Account>();
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check uniqueness
            var emailExists = await _db.Accounts.AnyAsync(a => a.Email == model.Email);
            var usernameExists = await _db.Accounts.AnyAsync(a => a.Username == model.Username);
            if (emailExists)
                ModelState.AddModelError(nameof(model.Email), "Email is already in use.");
            if (usernameExists)
                ModelState.AddModelError(nameof(model.Username), "Username is already taken.");
            if (!ModelState.IsValid)
                return View(model);

            var account = new Account
            {
                Username = model.Username,
                Email = model.Email,
                // other fields defaulted in Account (CreatedAt, IsActive, Role)
            };

            account.PasswordHash = _passwordHasher.HashPassword(account, model.Password);

            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Account created successfully. You can now sign in.";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var account = await _db.Accounts
                .FirstOrDefaultAsync(a => a.Email == model.UsernameOrEmail || a.Username == model.UsernameOrEmail);

            if (account == null || !account.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials.");
                return View(model);
            }

            var verifyResult = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, model.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role ?? "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            // indicate success so view/layout can show a message after redirect
            TempData["SuccessMessage"] = "Signed in successfully.";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}