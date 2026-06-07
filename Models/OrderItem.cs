namespace MakeupStore.Models
{
    // modelul pentru un element dintr-o comanda
    public class OrderItem
    {
        public int Id { get; set; }

        // comanda careia ii apartine
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // produsul comandat
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        // cantitatea comandata
        public int Quantity { get; set; }

        // pretul unitar la momentul plasarii comenzii
        public decimal UnitPrice { get; set; }
    }
}
