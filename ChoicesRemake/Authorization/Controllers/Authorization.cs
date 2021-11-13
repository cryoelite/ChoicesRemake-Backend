using Authorization.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Authorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Authorization : ControllerBase
    {
        private readonly ILogger<Authorization> _logger;
        private Consumer _consumer;

        public Authorization(ILogger<Authorization> logger, Consumer consumer)
        {
            _logger = logger;
            _consumer = consumer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = JsonSerializer.Serialize("");
            return new JsonResult(result);
        }
    }
}