namespace MakeupStore.Models
{
    // modelul pentru cosul de cumparaturi al unui utilizator
    public class Cart
    {
        public int Id { get; set; }

        // fiecare utilizator are un singur cos activ
        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // produsele din cos
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
