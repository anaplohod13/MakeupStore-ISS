namespace MakeupStore.Models
{
    // modelul pentru un element din cosul de cumparaturi
    public class CartItem
    {
        public int Id { get; set; }

        // cosul caruia ii apartine acest item
        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        // produsul adaugat in cos
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        // cantitatea selectata de utilizator
        public int Quantity { get; set; }
    }
}
