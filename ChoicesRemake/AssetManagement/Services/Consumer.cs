using KafkaService.Models;

namespace AssetManagement.Services
{
    public class Consumer
    {
        private readonly ILogger<Consumer> _logger;

        public Consumer(ILogger<Consumer> logger)
        {
            _logger = logger;
        }

        public async Task ManageMessage(KafkaData _kafkaData)
        {
            _logger.LogInformation("Processing a message in AssetManager");
        }
    }
}