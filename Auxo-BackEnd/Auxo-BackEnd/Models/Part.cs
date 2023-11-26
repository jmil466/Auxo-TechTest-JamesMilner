namespace Auxo_BackEnd.Models
{
    public class Part
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity {  get; set; }

        public Part(int id, string description, decimal price, int quantity)
        {
            Id = id;
            Description = description;
            Price = price;
            Quantity = quantity;
        }
    }
}
