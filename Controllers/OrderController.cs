using Microsoft.AspNetCore.Mvc;
using MakeupStore.Services;
using MakeupStore.ViewModels;

namespace MakeupStore.Controllers
{
    // controllerul pentru comenzi
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public OrderController(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        // metoda ajutatoare pentru a lua id-ul utilizatorului din sesiune
        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        // pagina de checkout - afisam rezumatul comenzii
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Order/Checkout" });
            }

            // luam cosul utilizatorului pentru a afisa rezumatul
            var cart = await _cartService.GetOrCreateCartAsync(userId.Value);

            // verificam daca cosul nu e gol
            if (!cart.CartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty. Please add products before checkout.";
                return RedirectToAction("Index", "Cart");
            }

            // pregatim view model-ul pentru checkout
            var viewModel = new CheckoutViewModel
            {
                CartItems = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "",
                    ProductBrand = ci.Product?.Brand ?? "",
                    ImageUrl = ci.Product?.ImageUrl ?? "",
                    UnitPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity
                }).ToList(),
                TotalAmount = cart.CartItems.Sum(ci => (ci.Product?.Price ?? 0) * ci.Quantity),
                ItemCount = cart.CartItems.Sum(ci => ci.Quantity)
            };

            return View(viewModel);
        }

        // procesam comanda - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                // reincarcam datele cosului pentru a putea afisa formularul din nou
                var cart = await _cartService.GetOrCreateCartAsync(userId.Value);
                model.CartItems = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "",
                    ProductBrand = ci.Product?.Brand ?? "",
                    ImageUrl = ci.Product?.ImageUrl ?? "",
                    UnitPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity
                }).ToList();
                model.TotalAmount = model.CartItems.Sum(i => i.TotalPrice);
                model.ItemCount = model.CartItems.Sum(i => i.Quantity);
                return View(model);
            }

            // plasam comanda si golim cosul
            var order = await _orderService.PlaceOrderAsync(userId.Value, model.ShippingAddress);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Could not place order. Please check your cart and try again.";
                return RedirectToAction("Index", "Cart");
            }

            // redirectionam la pagina de confirmare
            return RedirectToAction("Confirmation", new { id = order.Id });
        }

        // pagina de confirmare dupa plasarea comenzii
        public async Task<IActionResult> Confirmation(int id)
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // luam detaliile comenzii
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null || order.UserId != userId.Value)
            {
                return NotFound();
            }

            // transformam comanda in view model
            var viewModel = new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                Items = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name ?? "",
                    ImageUrl = oi.Product?.ImageUrl ?? "",
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            return View(viewModel);
        }

        // istoricul comenzilor utilizatorului
        public async Task<IActionResult> History()
        {
            // verificam daca utilizatorul e logat
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Order/History" });
            }

            // luam toate comenzile utilizatorului
            var orders = await _orderService.GetUserOrdersAsync(userId.Value);

            var viewModel = new OrderHistoryViewModel
            {
                Orders = orders.Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    ShippingAddress = o.ShippingAddress,
                    Items = o.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product?.Name ?? "",
                        ImageUrl = oi.Product?.ImageUrl ?? "",
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
