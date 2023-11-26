namespace Auxo_BackEnd.Models
{
    public class OrderResponse
    {
        public List<PartOrderLine> OrderLines { get; set; }
        public decimal Total { get; set; }

        public OrderResponse()
        {
            Total = 0;
            OrderLines = new List<PartOrderLine>();
        }
    }

    public class PartOrderLine
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
