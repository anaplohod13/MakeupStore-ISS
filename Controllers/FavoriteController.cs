using Microsoft.AspNetCore.Mvc;
using MakeupStore.Services;
using MakeupStore.ViewModels;

namespace MakeupStore.Controllers
{
    // controllerul pentru lista de favorite
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // metoda ajutatoare pentru id-ul utilizatorului
        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        // pagina cu lista de favorite
        public async Task<IActionResult> Index()
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Favorite" });
            }

            // luam favoritele utilizatorului
            var favorites = await _favoriteService.GetUserFavoritesAsync(userId.Value);

            // construim view model-ul
            var viewModel = new FavoriteViewModel
            {
                Favorites = favorites.Select(f => new FavoriteItemViewModel
                {
                    FavoriteId = f.Id,
                    ProductId = f.ProductId,
                    ProductName = f.Product?.Name ?? "",
                    ProductBrand = f.Product?.Brand ?? "",
                    Price = f.Product?.Price ?? 0,
                    ImageUrl = f.Product?.ImageUrl ?? "",
                    CategoryName = f.Product?.Category?.Name ?? "",
                    SavedAt = f.SavedAt,
                    IsInStock = (f.Product?.StockQuantity ?? 0) > 0
                }).ToList()
            };

            return View(viewModel);
        }

        // adaugam un produs la favorite
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, string? returnUrl)
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl ?? "/Favorite" });
            }

            // adaugam produsul la favorite
            await _favoriteService.AddToFavoritesAsync(userId.Value, productId);
            TempData["SuccessMessage"] = "Product saved to favorites.";

            // redirectionam inapoi la pagina de unde a venit
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }

        // stergem un produs din favorite
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId, string? returnUrl)
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // stergem produsul din lista de favorite
            await _favoriteService.RemoveFromFavoritesAsync(userId.Value, productId);
            TempData["SuccessMessage"] = "Product removed from favorites.";

            // redirectionam inapoi
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }
    }
}
