using Auxo_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auxo_BackEnd.Services
{
    public class PartsService : IPartsService
    {

        private List<Part> Parts = new List<Part>
        {
            new Part(1, "Wire", 5.99m, 5),
            new Part(2, "Brake Fluid", 4.90m, 20),
            new Part(3, "Engine Oil", 15.00m, 12)
        };

        private readonly ILogger<PartsService> _logger;

        public PartsService(ILogger<PartsService> logger)
        {
            _logger = logger;
        }

        public List<Part> GetParts()
        {
            return this.Parts;
        }

        public void AddPart(Part newPart)
        {
            var existingPart = Parts.FirstOrDefault(existingPart => existingPart.Id == newPart.Id);
            if(existingPart == null)
            {
                this.Parts.Add(newPart);
                return;
            }

            throw new Exception($"Part ID {newPart.Id} already exists!");
        }

        // This would likely live in its own Orders Service, but for ease of local data usage I've included it here
        public OrderResponse PlaceOrder(Order[] orders)
        {
            OrderResponse orderResponse = new OrderResponse();

            foreach (var order in orders)
            {
                var part = Parts.FirstOrDefault(part => part.Id == order.PartId);
                PartOrderLine orderLine = new PartOrderLine();

                if (part != null)
                {
                    if(order.OrderQuantity > part.Quantity) // Validates the order doesn't exceed available quantity.
                    {
                        // This could either not take any part of the order, fill to the max available, or 'skip' this part of the order.
                        // Current implementation skips that part of the order. 

                        _logger.LogInformation($"Order Part {order.PartId} exceeds available quantity");
                        orderLine = new PartOrderLine
                        {
                            Id = order.PartId,
                            Description = $"Order of {part.Description} exceeds available quantity. Number available: {part.Quantity}",
                            TotalPrice = 0
                        };
                    } else
                    {
                        orderLine = new PartOrderLine
                        {
                            Id = order.PartId,
                            Description = part.Description,
                            TotalPrice = (part.Price * order.OrderQuantity)
                        };

                        part.Quantity -= order.OrderQuantity;
                    }
                }
                else // Handling if an Unknown Part were to be appended
                {
                    _logger.LogInformation($"Part {order.PartId} is not a recongized part");

                    orderLine = new PartOrderLine
                    {
                        Id = order.PartId, // If multiple unknown parts are ordered, we can see what parts are unknown
                        Description = "Unknown Item",
                        TotalPrice = 0,
                    };
                }

                orderResponse.OrderLines.Add(orderLine);
                orderResponse.Total += orderLine.TotalPrice;
            }

            return orderResponse;
        }


    }
}
