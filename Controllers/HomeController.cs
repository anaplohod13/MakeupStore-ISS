using Microsoft.AspNetCore.Mvc;
using MakeupStore.Services;
using MakeupStore.ViewModels;

namespace MakeupStore.Controllers
{
    // controllerul paginii principale
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        // pagina principala - afisam produsele recente
        public async Task<IActionResult> Index()
        {
            // luam toate produsele pentru pagina de start
            var products = await _productService.GetAllProductsAsync();

            // transformam produsele in view models pentru afisare
            var productViewModels = products.Take(6).Select(p => new ProductViewModel
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

            return View(productViewModels);
        }
    }
}
