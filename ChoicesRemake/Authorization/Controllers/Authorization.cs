using Authorization.Services;
using KafkaService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace Authorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Authorization : ControllerBase
    {
        private readonly ILogger<Authorization> _logger;
        private Consumer _consumer;
        private IServiceProvider service;
        public Authorization(ILogger<Authorization> logger, Consumer consumer, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _consumer = consumer;
            service = serviceProvider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = JsonSerializer.Serialize(_consumer.outps);
            return new JsonResult(result);
        }

        [HttpGet("useCount")]
        public IActionResult UseCount()
        {
            int val;

            using (var scope = service.CreateScope())
            {
                var kafka = scope.ServiceProvider.GetRequiredService<KafkaConsumer>();
                kafka.useCount.TryTake(out val);
            }
            var result = JsonSerializer.Serialize(val);
            return new JsonResult(result);
        }
    }
}