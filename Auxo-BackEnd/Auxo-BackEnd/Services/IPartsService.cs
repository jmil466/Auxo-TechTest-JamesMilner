using Auxo_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auxo_BackEnd.Services
{
    public interface IPartsService
    {
        void AddPart(Part part);
        List<Part> GetParts();
        OrderResponse PlaceOrder(Order[] orders);
    }
}