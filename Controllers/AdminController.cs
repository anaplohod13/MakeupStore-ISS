using Microsoft.AspNetCore.Mvc;
using MakeupStore.Services;
using MakeupStore.ViewModels;
using MakeupStore.Models;

namespace MakeupStore.Controllers
{
    // controllerul pentru panoul de administrare - CRUD produse
    public class AdminController : Controller
    {
        private readonly IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        // metoda ajutatoare - verificam daca utilizatorul e admin
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        // verificare acces admin - redirectionam daca nu e autorizat
        private IActionResult? CheckAdminAccess()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Admin" });
            }
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            return null;
        }

        // pagina principala admin - lista de produse
        public async Task<IActionResult> Index()
        {
            var redirect = CheckAdminAccess();
            if (redirect != null) return redirect;

            // luam toate produsele pentru administrare
            var products = await _productService.GetAllProductsAsync();

            var viewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Brand = p.Brand,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? ""
            }).ToList();

            return View(viewModels);
        }

        // formularul de creare produs - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var redirect = CheckAdminAccess();
            if (redirect != null) return redirect;

            // luam categoriile pentru dropdown
            var categories = await _productService.GetAllCategoriesAsync();
            var viewModel = new AdminProductViewModel
            {
                Categories = categories
            };

            return View(viewModel);
        }

        // procesam crearea produsului - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminProductViewModel model)
        {
            var redirect = CheckAdminAccess();
            if (redirect != null) return redirect;

            if (!ModelState.IsValid)
            {
                // reincarcam categoriile in caz de eroare
                model.Categories = await _productService.GetAllCategoriesAsync();
                return View(model);
            }

            // cream produsul nou
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Brand = model.Brand,
                StockQuantity = model.StockQuantity,
                ImageUrl = string.IsNullOrEmpty(model.ImageUrl) ? "/images/placeholder.jpg" : model.ImageUrl,
                CategoryId = model.CategoryId
            };

            // salvam produsul in baza de date
            await _productService.AddProductAsync(product);

            TempData["SuccessMessage"] = $"Product '{model.Name}' has been created successfully.";
            return RedirectToAction("Index");
        }

        // formularul de editare produs - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var redirect = CheckAdminAccess();
            if (redirect != null) return redirect;

            // luam produsul pentru editare
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // luam categoriile pentru dropdown
            var categories = await _productService.GetAllCategoriesAsync();

            var viewModel = new AdminProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Brand = product.Brand,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Categories = categories
            };

            return View(viewModel);
        }

        // procesam editarea produsului - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminProductViewModel model)
        {
            var redirect = CheckAdminAccess();
            if (redirect != null) return redirect;

            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // reincarcam categoriile in caz de eroare
                model.Categories = await _productService.GetAllCategoriesAsync();
                return View(model);
            }

            // verificam daca produsul exista
            if (!await _productService.ProductExistsAsync(id))
            {
                return NotFound();
            }

            // actualizam produsul cu datele noi
            var product = new Product
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Brand = model.Brand,
                StockQuantity = model.StockQuantity,
                ImageUrl = string.IsNullOrEmpty(model.ImageUrl) ? "/images/placeholder.jpg" : model.ImageUrl,
                CategoryId = model.CategoryId
            };

            // salvam modificarile
            await _productService.UpdateProductAsync(product);

            TempData["SuccessMessage"] = $"Product '{model.Name}' has been updated successfully.";
            return RedirectToAction("Index");
        }

        // pagina de confirmare stergere - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var redirect = CheckAdminAccess();
            if (redirect != null) return redirect;

            // luam produsul pentru confirmare stergere
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Brand = product.Brand,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? ""
            };

            return View(viewModel);
        }

        // procesam stergerea produsului - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var redirect = CheckAdminAccess();
            if (redirect != null) return redirect;

            // verificam daca produsul exista
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // stergem produsul din baza de date
            await _productService.DeleteProductAsync(id);

            TempData["SuccessMessage"] = $"Product '{product.Name}' has been deleted.";
            return RedirectToAction("Index");
        }
    }
}
