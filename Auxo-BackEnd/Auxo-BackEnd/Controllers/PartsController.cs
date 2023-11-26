using Auxo_BackEnd.Models;
using Auxo_BackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auxo_BackEnd.Controllers
{

    [ApiController]
    [Route("parts")]
    public class PartsController : Controller
    {

        private readonly ILogger<PartsController> _logger;
        private readonly PartsService _partsService;
        public PartsController(ILogger<PartsController> logger, PartsService partsService) {
            _logger = logger;
            _partsService = partsService;
        }

        [HttpGet]
        public IActionResult GetParts()
        {
            try
            {
                _logger.LogInformation($"Beginning Process {nameof(PartsController)}.{nameof(GetParts)}");

                var parts = _partsService.GetParts();

                _logger.LogInformation($"Process Completed {nameof(PartsController)}.{nameof(GetParts)}");
                return Ok(parts);
            } catch(Exception ex)
            {
                _logger.LogError($"An Exception Occurred in {nameof(PartsController)}.{nameof(GetParts)} - Error {ex}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPost]
        public IActionResult AddPart(Part part)
        {

            try
            {
                _logger.LogInformation($"Beginning Process {nameof(PartsController)}.{nameof(AddPart)}");
                
                _partsService.AddPart(part);
                var parts = _partsService.GetParts();

                _logger.LogInformation($"Process Completed {nameof(PartsController)}.{nameof(GetParts)}");
                return Ok(parts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Exception Occurred in {nameof(PartsController)}.{nameof(AddPart)} - Error {ex}");
                return StatusCode(500, "Internal Server Error");
            }

        }
    }
}
