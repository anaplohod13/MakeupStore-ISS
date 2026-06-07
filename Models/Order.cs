namespace MakeupStore.Models
{
    // modelul pentru o comanda plasata de utilizator
    public class Order
    {
        public int Id { get; set; }

        // utilizatorul care a plasat comanda
        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // suma totala a comenzii
        public decimal TotalAmount { get; set; }

        // statusul comenzii
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // adresa de livrare introdusa de utilizator
        public string ShippingAddress { get; set; } = string.Empty;

        // produsele incluse in comanda
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
