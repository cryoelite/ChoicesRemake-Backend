using Authentication.Settings;
using AuthenticationIdentityDB;
using AuthenticationIdentityModel;
using KafkaService.Models;
using KafkaService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StaticAssets;
using System;
using System.Text;

namespace Authentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication v1"));
            }

/*            app.UseHttpsRedirection();*/

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = new JWTSettings()
            {
                DataEncryptionAlgorithm = Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_DataEncryptionAlgorithm),
                EncryptionKey = Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_EncryptionKey),
                ExpirationInDays = Configuration.GetValue<int>(ConfigurationKeys.authentication_jwt_expiration),
                Issuer = Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_issuer),
                KeyWrapAlgorithm = Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_KeyWrapAlgorithm),
                SigningAlgorithm = Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_SigningAlgorithm),
                SigningKey = Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_SigningKey),
            };
            services.AddSingleton((_) => jwtSettings);

            var connStr = Configuration.GetValue<string>(ConfigurationKeys.authentication_connectionString);

            services.AddDbContext<ApplicationDBContext>(o => o.UseSqlServer(connStr));

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var byteArray = new byte[32];
                Array.Copy(Encoding.UTF8.GetBytes(jwtSettings.EncryptionKey), byteArray, 32);
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
                    TokenDecryptionKey = new SymmetricSecurityKey(byteArray),
                    ClockSkew = TimeSpan.Zero,
                    ValidAlgorithms = new[] { jwtSettings.DataEncryptionAlgorithm, jwtSettings.KeyWrapAlgorithm, jwtSettings.SigningAlgorithm },
                };
            });

            services.AddIdentityCore<ApplicationUser>(o =>
            {
                o.Stores.MaxLengthForKeys = 128;
                o.SignIn.RequireConfirmedAccount = true;
                o.Password.RequiredLength = 8;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDBContext>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication", Version = "v1" });
                /*c.AddSecurityDefinition(WebAPI_Headers.backerToken, new OpenApiSecurityScheme
                {
                    Description = "Backer Bearer Token for admin/vendor registeration",
                    Name = WebAPI_Headers.backerToken,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });*/

                c.OperationFilter<SwaggerConfFilter>();
            });
           string brokerURL = Configuration.GetValue<string>(ConfigurationKeys.kafka_broker1);
            string topicPrimary = Configuration.GetValue<string>(ConfigurationKeys.kafka_assetTopicPrimary);
            string topicSecondary = Configuration.GetValue<string>(ConfigurationKeys.kafka_assetTopicSecondary);
            string groupSecondary = Configuration.GetValue<string>(ConfigurationKeys.kafka_assetGroupSecondary);
            string groupPrimary = Configuration.GetValue<string>(ConfigurationKeys.kafka_assetGroupPrimary);

            services.AddSingleton<KafkaProducer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<KafkaProducer>>();
                var kafkaConfig = new KafkaConfiguration(topicPrimary, brokerURL, groupPrimary, "Authentication", topicSecondary, groupSecondary);
                var asynckafkaProducer = KafkaProducer.BuildProducer(logger, kafkaConfig);
                var kafkaProducer = asynckafkaProducer.GetAwaiter().GetResult();
                return kafkaProducer;
            });
        }
    }
}