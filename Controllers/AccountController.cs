using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPTEST.Data;
using UPTEST.Models;
using UPTEST.ViewModels;

namespace UPTEST.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public AccountController(ApplicationDbContext context, PasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Auth(string tab = "login")
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new AuthPageViewModel
        {
            ActiveTab = tab == "register" ? "register" : "login"
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind(Prefix = "Login")] LoginViewModel login, string? returnUrl = null)
    {
        var model = new AuthPageViewModel
        {
            Login = login,
            ActiveTab = "login"
        };

        if (!ModelState.IsValid)
        {
            return View("Auth", model);
        }

        var email = login.Email.Trim().ToLowerInvariant();
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == email);

        if (user == null || !user.IsActive)
        {
            ModelState.AddModelError("Login.Email", "Пользователь не найден или деактивирован.");
            return View("Auth", model);
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? string.Empty, login.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("Login.Password", "Неверный пароль.");
            return View("Auth", model);
        }

        user.LastLoginDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        await SignInAsync(user, login.RememberMe);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([Bind(Prefix = "Register")] RegisterViewModel register)
    {
        var model = new AuthPageViewModel
        {
            Register = register,
            ActiveTab = "register"
        };

        if (!ModelState.IsValid)
        {
            return View("Auth", model);
        }

        var email = register.Email.Trim().ToLowerInvariant();
        var emailExists = await _context.Users.AnyAsync(u => u.Email != null && u.Email.ToLower() == email);
        if (emailExists)
        {
            ModelState.AddModelError("Register.Email", "Пользователь с таким email уже существует.");
            return View("Auth", model);
        }

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Менеджер")
                   ?? await _context.Roles.OrderBy(r => r.RoleId).FirstAsync();

        var user = new User
        {
            FullName = register.FullName.Trim(),
            Email = email,
            RoleId = role.RoleId,
            PhoneNumber = string.IsNullOrWhiteSpace(register.PhoneNumber) ? null : register.PhoneNumber.Trim(),
            Address = string.IsNullOrWhiteSpace(register.Address) ? null : register.Address.Trim(),
            RegistrationDate = DateTime.UtcNow,
            IsActive = true
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, register.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        user.Role = role;
        await SignInAsync(user, false);

        TempData["StatusMessage"] = "Регистрация прошла успешно. Добро пожаловать в систему!";
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Auth));
    }

    private async Task SignInAsync(User user, bool rememberMe)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.FullName ?? user.Email ?? "Пользователь"),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Role, user.Role?.RoleName ?? "Сотрудник")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(14) : null
            });
    }
}
