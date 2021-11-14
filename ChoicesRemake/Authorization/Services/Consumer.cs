using AuthorizationDBLayer;
using AuthorizationModel;
using AuthorizationRepository;
using IAuthorizationRepository;
using KafkaService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StaticAssets;
using System.Threading.Tasks;

namespace Authorization.Services
{
    public class Consumer
    {
        public JWTDecryptor jwtDecryptor;
        private readonly ILogger<Consumer> _logger;
        private IAuthorizationRepo repo;

        public Consumer(DbContextOptions<AuthorizationDBContext> options, ILogger<AuthorizationRepo> authLogger, ILogger<Consumer> logger, JWTDecryptor jWTDecryptor)
        {

            var adb = new AuthorizationDBContext(options);
            repo = new AuthorizationRepo(adb, authLogger);
            _logger = logger;
            this.jwtDecryptor = jWTDecryptor;
        }

        public async Task ManageMessage(KafkaData _kafkaData)
        {
            _logger.LogInformation("Processing a message in Authorization");
            if (_kafkaData.GetMethodName() == MethodNames.addUser)
            {
                var userRole = new UserRole();
                var token = _kafkaData.GetCustomHeader(WebAPI_Headers.bearerToken);
                var role = _kafkaData.GetCustomHeader(Role.role);
                if (token == null || role == null)
                {
                    throw new System.Exception($"Kafka Message missing {(token == null ? WebAPI_Headers.bearerToken : Role.role)} header");
                }

                userRole.username = jwtDecryptor.GetUsername(token);
                userRole.role = role;
                await repo.addNewUser(userRole);
            }
        }
    }
}