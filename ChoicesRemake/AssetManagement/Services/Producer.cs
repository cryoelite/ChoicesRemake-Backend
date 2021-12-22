using KafkaService.Models;
using StaticAssets;

namespace AssetManagement.Services
{
    public class Producer
    {
        private readonly ILogger<Producer> _logger;
        private string gatewayURL;
        private IWebHostEnvironment webHostEnvironment;

        public Producer(ILogger<Producer> logger, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            gatewayURL = configuration.GetValue<string>(ConfigurationKeys.assetManager_Gateway);
        }

        public async Task<KafkaData> ManageMessage(KafkaData _kafkaData)
        {
            _logger.LogInformation("Request received in AssetManager");
            _kafkaData.MarkFailure();

            if (_kafkaData.GetMethodName() == MethodNames.storeImageAndGetPath)
            {
                var imageType = _kafkaData.GetCustomHeader(CustomHeader.fileType);
                var fileBrand = _kafkaData.GetCustomHeader(CustomHeader.fileBrand);
                if (imageType != null && fileBrand != null)
                {
                    var imageData = _kafkaData.GetCustomRawHeader(CustomHeader.imageKey);
                    if (imageData != null)
                    {
                        var tempPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images");
                        var tempPath2 = Path.Combine(tempPath, fileBrand);
                        Directory.CreateDirectory(tempPath2);
                        var actualPath = Path.Combine(tempPath2, string.Concat(Path.GetRandomFileName(), imageType));
                        using (var bw = new BinaryWriter(File.OpenWrite(actualPath)))
                        {
                            bw.Write(imageData);
                        }
                        var imagePath = actualPath.Replace("/app", "");
                        imagePath = String.Concat(gatewayURL, imagePath);
                        _kafkaData.RemoveHeader(CustomHeader.imageKey);
                        _kafkaData.AddHeader(CustomHeader.imageLocation, imagePath);
                        _kafkaData.MarkSuccess();
                    }
                }
            }
            _logger.LogInformation($"Returning {_kafkaData.message.Value} from AssetManager");

            return _kafkaData;
        }
    }
}