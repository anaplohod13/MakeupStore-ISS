using System.ComponentModel.DataAnnotations;
using MakeupStore.Models;

namespace MakeupStore.ViewModels
{
    // view model pentru formularele admin de creare/editare produs
    public class AdminProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, 10000, ErrorMessage = "Stock quantity must be between 0 and 10000")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // categoriile disponibile pentru dropdown
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
