using Microsoft.AspNetCore.Mvc;
using MakeupStore.Services;
using MakeupStore.ViewModels;

namespace MakeupStore.Controllers
{
    // controllerul pentru produse - catalog, cautare, filtrare, detalii
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IFavoriteService _favoriteService;

        public ProductController(IProductService productService, IFavoriteService favoriteService)
        {
            _productService = productService;
            _favoriteService = favoriteService;
        }

        // pagina catalog cu cautare si filtrare combinate
        public async Task<IActionResult> Index(string? searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            // luam categoriile pentru filtrul din sidebar
            var categories = await _productService.GetAllCategoriesAsync();

            IEnumerable<Models.Product> products;

            // daca avem termen de cautare, folosim cautarea
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                products = await _productService.SearchProductsAsync(searchTerm);
                // aplicam si filtrele peste rezultatele cautarii
                if (categoryId.HasValue)
                    products = products.Where(p => p.CategoryId == categoryId.Value);
                if (minPrice.HasValue)
                    products = products.Where(p => p.Price >= minPrice.Value);
                if (maxPrice.HasValue)
                    products = products.Where(p => p.Price <= maxPrice.Value);
            }
            else if (categoryId.HasValue || minPrice.HasValue || maxPrice.HasValue)
            {
                // aplicam doar filtrele fara cautare
                products = await _productService.FilterProductsAsync(categoryId, minPrice, maxPrice);
            }
            else
            {
                // luam toate produsele daca nu avem criterii
                products = await _productService.GetAllProductsAsync();
            }

            // verificam daca utilizatorul e logat pentru a afisa favoritele
            var userId = HttpContext.Session.GetInt32("UserId");

            // transformam produsele in view models
            var productViewModels = new List<ProductViewModel>();
            foreach (var p in products)
            {
                var isFavorite = false;
                if (userId.HasValue)
                {
                    isFavorite = await _favoriteService.IsProductFavoriteAsync(userId.Value, p.Id);
                }

                productViewModels.Add(new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Brand = p.Brand,
                    StockQuantity = p.StockQuantity,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.Name ?? "",
                    IsFavorite = isFavorite
                });
            }

            // construim view model-ul pentru lista de produse
            var viewModel = new ProductListViewModel
            {
                Products = productViewModels,
                Categories = categories,
                SearchTerm = searchTerm,
                SelectedCategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                TotalCount = productViewModels.Count
            };

            return View(viewModel);
        }

        // pagina de detalii pentru un produs specific
        public async Task<IActionResult> Details(int id)
        {
            // luam produsul din baza de date
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // verificam daca utilizatorul a salvat produsul la favorite
            var userId = HttpContext.Session.GetInt32("UserId");
            var isFavorite = false;
            if (userId.HasValue)
            {
                isFavorite = await _favoriteService.IsProductFavoriteAsync(userId.Value, id);
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
                CategoryName = product.Category?.Name ?? "",
                IsFavorite = isFavorite
            };

            return View(viewModel);
        }
    }
}
