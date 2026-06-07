namespace MakeupStore.ViewModels
{
    // view model pentru un element din cosul de cumparaturi
    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductBrand { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        // pretul total pentru acest element (pret * cantitate)
        public decimal TotalPrice => UnitPrice * Quantity;
    }

    // view model pentru afisarea intregului cos de cumparaturi
    public class CartViewModel
    {
        // lista elementelor din cos
        public IEnumerable<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        // totalul general al cosului
        public decimal GrandTotal => Items.Sum(i => i.TotalPrice);

        // numarul total de produse din cos
        public int ItemCount => Items.Sum(i => i.Quantity);

        // verificam daca cosul e gol
        public bool IsEmpty => !Items.Any();
    }
}
