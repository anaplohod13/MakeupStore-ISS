using Microsoft.AspNetCore.Mvc;
using MakeupStore.Services;
using MakeupStore.ViewModels;

namespace MakeupStore.Controllers
{
    // controllerul pentru cosul de cumparaturi
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        // metoda ajutatoare - verificam daca utilizatorul e logat
        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        // pagina cosului de cumparaturi
        public async Task<IActionResult> Index()
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Cart" });
            }

            // luam cosul utilizatorului
            var cart = await _cartService.GetOrCreateCartAsync(userId.Value);

            // transformam datele in view model
            var viewModel = new CartViewModel
            {
                Items = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "",
                    ProductBrand = ci.Product?.Brand ?? "",
                    ImageUrl = ci.Product?.ImageUrl ?? "",
                    UnitPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity
                }).ToList()
            };

            return View(viewModel);
        }

        // adaugam un produs in cos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = $"/Product/Details/{productId}" });
            }

            // verificam daca produsul exista
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // adaugam produsul in cos
            await _cartService.AddToCartAsync(userId.Value, productId, quantity);

            TempData["SuccessMessage"] = $"{product.Name} has been added to your cart.";
            return RedirectToAction("Index");
        }

        // actualizam cantitatea unui produs din cos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // actualizam cantitatea
            await _cartService.UpdateCartItemAsync(cartItemId, quantity);
            return RedirectToAction("Index");
        }

        // stergem un produs din cos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // stergem produsul din cos
            await _cartService.RemoveFromCartAsync(cartItemId);
            TempData["SuccessMessage"] = "Item removed from cart.";
            return RedirectToAction("Index");
        }
    }
}
