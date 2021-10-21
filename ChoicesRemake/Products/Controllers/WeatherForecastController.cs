using IProductsDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsDBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductsDB;

namespace Products.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        ProductInterface productInterface;
        public WeatherForecastController(ProductsDbContext pdb) => productInterface = new ProductMethods(pdb);
        

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await productInterface.searchAndGetProductsByName("Standard Plate");
            return new JsonResult(product);


        }
    }
}
