namespace Auxo_BackEnd.Models
{
    public class Order
    {

        // In a real application, an order would have an ID, as it would be tracked on a DB level, but I haven't included it in this test

        public int PartId { get; set; }
        public int OrderQuantity { get; set; }

        public Order(int partId, int orderQuantity)
        {
            PartId = partId;
            OrderQuantity = orderQuantity;
        }
    }
}
