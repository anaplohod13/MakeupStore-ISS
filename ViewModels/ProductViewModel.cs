namespace MakeupStore.ViewModels
{
    // view model pentru afisarea detaliilor unui produs
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Brand { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        // verificam daca produsul e disponibil in stoc
        public bool IsInStock => StockQuantity > 0;

        // verificam daca utilizatorul logat a salvat produsul la favorite
        public bool IsFavorite { get; set; }
    }
}
