namespace MakeupStore.Models
{
    // modelul pentru categoriile de produse cosmetice
    public class Category
    {
        public int Id { get; set; }

        // numele categoriei (ex: Foundation, Lipstick, Eyeshadow)
        public string Name { get; set; } = string.Empty;

        // lista de produse din aceasta categorie
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
