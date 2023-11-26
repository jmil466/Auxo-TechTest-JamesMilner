using Auxo_BackEnd.Models;
using Auxo_BackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auxo_BackEnd.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly PartsService _partsService;
        public OrderController(ILogger<OrderController> logger, PartsService partsService)
        {
            _logger = logger;
            _partsService = partsService;
        }

        [HttpPost]
        public IActionResult PlaceOrder(Order[] orders)
        {
            try
            {
                _logger.LogInformation($"Beginning Process {nameof(OrderController)}.{nameof(PlaceOrder)}");

                var order = _partsService.PlaceOrder(orders);

                _logger.LogInformation($"Process Completed {nameof(OrderController)}.{nameof(PlaceOrder)}");
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Exception Occurred in {nameof(OrderController)}.{nameof(PlaceOrder)} - Error {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
