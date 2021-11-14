﻿using AuthorizationDBLayer;
using AuthorizationRepository;
using IAuthorizationRepository;
using KafkaService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StaticAssets;
using System.Threading.Tasks;

namespace Authorization.Services
{
    public class Producer
    {
        public JWTDecryptor jwtDecryptor;
        private readonly ILogger<Producer> _logger;
        private IAuthorizationRepo repo;

        public Producer(DbContextOptions<AuthorizationDBContext> options,ILogger<AuthorizationRepo> authLogger, ILogger<Producer> logger, JWTDecryptor jWTDecryptor)
        {

            var adb = new AuthorizationDBContext(options);
            repo = new AuthorizationRepo(adb,authLogger);
            _logger = logger;
            this.jwtDecryptor = jWTDecryptor;
        }

        public async Task<KafkaData> ManageMessage(KafkaData _kafkaData)
        {
            _logger.LogInformation("Request received in Authorization");
            _kafkaData.MarkFailure();

            if (_kafkaData.GetMethodName() == MethodNames.verifyAdmin)
            {
                var backerToken = _kafkaData.GetCustomHeader(WebAPI_Headers.backerToken);
                if (backerToken != null)
                {
                    var username = jwtDecryptor.GetUsername(backerToken);
                    if (username != null)
                    {
                        var role = await repo.getUser(username);
                        if (role == Role.admin)
                        {
                            _logger.LogInformation($"{username} indeed has role {Role.admin}");

                            _kafkaData.MarkSuccess();
                        }
                    }
                }
            }

            return _kafkaData;
        }
    }
}