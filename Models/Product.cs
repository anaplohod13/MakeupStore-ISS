namespace MakeupStore.Models
{
    // modelul pentru produsele din magazin
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // pretul produsului
        public decimal Price { get; set; }

        // brandul producatorului
        public string Brand { get; set; } = string.Empty;

        // cantitatea disponibila in stoc
        public int StockQuantity { get; set; }

        // url-ul imaginii produsului
        public string ImageUrl { get; set; } = string.Empty;

        // cheia straina catre categorie
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // relatii cu celelalte entitati
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
