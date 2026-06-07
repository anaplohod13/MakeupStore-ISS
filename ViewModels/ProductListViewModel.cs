using MakeupStore.Models;

namespace MakeupStore.ViewModels
{
    // view model pentru lista de produse cu filtre si cautare
    public class ProductListViewModel
    {
        // lista de produse de afisat
        public IEnumerable<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();

        // categoriile disponibile pentru filtrare
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        // termenul de cautare introdus de utilizator
        public string? SearchTerm { get; set; }

        // filtrul de categorie selectat
        public int? SelectedCategoryId { get; set; }

        // pretul minim pentru filtrare
        public decimal? MinPrice { get; set; }

        // pretul maxim pentru filtrare
        public decimal? MaxPrice { get; set; }

        // numarul total de produse gasite
        public int TotalCount { get; set; }
    }
}
