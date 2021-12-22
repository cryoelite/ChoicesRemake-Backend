using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Controllers
{
    [ApiController]
    [Route("asset")]
    public class AssetManager : ControllerBase
    {
        private ILogger logger;

        public AssetManager(ILogger<AssetManager> logger)
        {
            this.logger = logger;
        }

        [HttpPost("advertData")]
        public IActionResult AdvertData()
        {
            //Store a few files in api, like banner, image, adverts, etc. Then return them using this API. Since this is just
            //a personal project I will hard code them in the front end but otherwise get them from here.

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            //Store a few files in api, like banner, image, adverts, etc. Then return them using this API. Since this is just
            //a personal project I will hard code them in the front end but otherwise get them from here.

            return Ok();
        }
    }
}