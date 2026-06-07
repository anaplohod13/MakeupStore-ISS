namespace MakeupStore.Models
{
    // modelul pentru produsele favorite ale unui utilizator
    public class Favorite
    {
        public int Id { get; set; }

        // utilizatorul care a salvat produsul
        public int UserId { get; set; }
        public User? User { get; set; }

        // produsul salvat ca favorit
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        // data la care a fost salvat produsul
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}
