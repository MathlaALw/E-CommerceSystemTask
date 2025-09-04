using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSystem.Controllers
{
   
        [ApiController]
        [Route("api/[controller]")]
        public class HealthController : ControllerBase
        {
            private readonly IAppLogger<HealthController> _logger;

            public HealthController(IAppLogger<HealthController> logger)
            {
                _logger = logger;
            }

            [HttpGet]
            public IActionResult Get()
            {
                _logger.LogInformation("Health check endpoint called");
                return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
            }
        
        }
}
