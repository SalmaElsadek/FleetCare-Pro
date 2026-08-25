using FleetCare_Pro.Models;
using FleetCare_Pro.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FleetCare_Pro.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Authentication> _userManager;
        private readonly SignInManager<Authentication> _signInManager;

        public AccountController(UserManager<Authentication> userManager, SignInManager<Authentication> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Login 
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        // بتأكد ان الريكوست جاي من شاشتي مش من هاكر عشان احمي نفسي من ال CSRF (Cross-Site Request Forgery)
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Login failed, make sure your email and password are correct.");
            }
            return View(model);
        }

        //  GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string prefix = model.Role == "FleetManager" ? "MAN" : "EMP";

                
                var usersInRole = await _userManager.GetUsersInRoleAsync(model.Role);
                int nextNumber = usersInRole.Count + 1001;

                string generatedEmployeeId = $"{prefix}-{nextNumber}";

                var user = new Authentication
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    EmployeeId = generatedEmployeeId 
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        //POST: Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
