namespace MakeupStore.ViewModels
{
    // view model pentru afisarea produselor favorite
    public class FavoriteItemViewModel
    {
        public int FavoriteId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductBrand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; }
        public bool IsInStock { get; set; }
    }

    // view model pentru pagina de favorite
    public class FavoriteViewModel
    {
        // lista produselor favorite ale utilizatorului
        public IEnumerable<FavoriteItemViewModel> Favorites { get; set; } = new List<FavoriteItemViewModel>();

        // numarul de produse la favorite
        public int Count => Favorites.Count();

        // verificam daca lista e goala
        public bool IsEmpty => !Favorites.Any();
    }
}
