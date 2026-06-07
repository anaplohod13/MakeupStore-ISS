using System.ComponentModel.DataAnnotations;

namespace MakeupStore.ViewModels
{
    // view model pentru pagina de checkout (finalizare comanda)
    public class CheckoutViewModel
    {
        // adresa de livrare introdusa de utilizator
        [Required(ErrorMessage = "Shipping address is required")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; } = string.Empty;

        // informatii despre cos pentru afisare
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        // totalul comenzii
        public decimal TotalAmount { get; set; }

        // numarul de produse din cos
        public int ItemCount { get; set; }
    }
}
