using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAuthorizationRepository;
using AuthorizationDBLayer;
using AuthorizationRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Authorization.Services
{
    public class Consumer
    {
        public List<string> outps = new List<string>();
        private IAuthorizationRepo repo;
        private readonly ILogger<Consumer> _logger;
        public Consumer(DbContextOptions<AuthorizationDBContext> options, ILogger<Consumer> logger)
        {
            var adb = new AuthorizationDBContext(options);
            repo = new AuthorizationRepo(adb);
            _logger = logger;
        }

        public async Task ManageMessage(KeyValuePair<string, string> message, Dictionary<string, string>? headers)
        {

            outps.Add($"Message recieved is ${message.Key} with ${message.Value}");
            if (headers != null)
            {
                foreach (var elem in headers) 
                {
                    outps.Add($"Header key {elem.Key} and {elem.Value}");
                }
            }
        }
    }
}
