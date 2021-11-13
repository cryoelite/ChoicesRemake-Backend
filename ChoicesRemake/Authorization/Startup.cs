using Authorization.Services;
using AuthorizationDBLayer;
using KafkaService.Models;
using KafkaService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StaticAssets;

namespace Authorization
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JWTSettings>(Configuration.GetSection(ConfigurationKeys.authenticationSection).GetSection(ConfigurationKeys.authentication_jwt));

            var connStr = Configuration.GetSection(ConfigurationKeys.authorizationSection)[ConfigurationKeys.connectionString];
            services.AddDbContext<AuthorizationDBContext>(o => o.UseSqlServer(connStr));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authorization", Version = "v1" });
            });

            //not using options pattern for no reason lmao
            var kafkaSection = Configuration.GetSection(ConfigurationKeys.kafkaSection);
            string brokerURL = kafkaSection[ConfigurationKeys.kafka_broker1];
            string topicPrimary = kafkaSection[ConfigurationKeys.kafka_authTopicPrimary];
            string topicSecondary = kafkaSection[ConfigurationKeys.kafka_authTopicSecondary];
            string groupSecondary = kafkaSection[ConfigurationKeys.kafka_authGroupSecondary];
            string groupPrimary = kafkaSection[ConfigurationKeys.kafka_authGroupPrimary];

            services.AddSingleton((_) =>
            {
                var dbContextOptions = new DbContextOptionsBuilder<AuthorizationDBContext>();
                dbContextOptions.UseSqlServer(connStr);

                return dbContextOptions.Options;
            });
            services.AddSingleton<Consumer>();
            services.AddSingleton<Producer>();

            services.AddHostedService<KafkaConsumer>(sp =>
            {
                var producer = sp.GetRequiredService<Producer>();
                var consumer = sp.GetRequiredService<Consumer>();
                var config = new KafkaConfiguration(ctortopicPrimary: topicPrimary, ctorbrokerURL: brokerURL, ctorgroupPrimary: groupPrimary, clientName: "Authorization", ctorgroupSecondary: groupSecondary, ctortopicSecondary: topicSecondary);
                var logger = sp.GetRequiredService<ILogger<KafkaConsumer>>();
                return new KafkaConsumer(logger, config, consumer.ManageMessage, producer.ManageMessage);
            });

            services.AddSingleton<JWTDecryptor>();
        }
    }
}