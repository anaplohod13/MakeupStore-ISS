using Microsoft.AspNetCore.Mvc;
using MakeupStore.Services;
using MakeupStore.ViewModels;

namespace MakeupStore.Controllers
{
    // controllerul pentru autentificare si inregistrare
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // pagina de login - GET
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            // daca utilizatorul e deja logat, il redirectionam
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new LoginViewModel { ReturnUrl = returnUrl };
            return View(viewModel);
        }

        // procesam formularul de login - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // verificam credentialele utilizatorului
            var user = await _userService.LoginAsync(model.Email, model.Password);
            if (user == null)
            {
                // credentiale incorecte
                ModelState.AddModelError("", "Invalid email or password. Please try again.");
                return View(model);
            }

            // salvam datele utilizatorului in sesiune
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            // redirectionam la pagina ceruta sau la home
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // pagina de inregistrare - GET
        [HttpGet]
        public IActionResult Register()
        {
            // daca utilizatorul e deja logat, il redirectionam
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        // procesam formularul de inregistrare - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // verificam daca emailul e deja folosit
            if (await _userService.EmailExistsAsync(model.Email))
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(model);
            }

            // inregistram utilizatorul nou
            var user = await _userService.RegisterAsync(model.Email, model.Password, model.FirstName, model.LastName);
            if (user == null)
            {
                ModelState.AddModelError("", "Registration failed. Please try again.");
                return View(model);
            }

            // logam automat utilizatorul dupa inregistrare
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            TempData["SuccessMessage"] = "Welcome! Your account has been created successfully.";
            return RedirectToAction("Index", "Home");
        }

        // delogarea utilizatorului
        public IActionResult Logout()
        {
            // golim sesiunea
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
