using IProductsRepository;
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
using nClam;
using ProductsDBLayer;
using ProductsRepository;
using StaticAssets;

namespace Products
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products v1"));
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
            var connStr = Configuration.GetSection(ConfigurationKeys.productsSection)[ConfigurationKeys.connectionString];
            services.AddDbContext<ProductsDBContext>(o => o.UseSqlServer(connStr));
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddControllers(options =>
            {
                //options.InputFormatters.Insert(0, new ImageTypeConverter());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products", Version = "v1" });
            });

            var kafkaSection = Configuration.GetSection(ConfigurationKeys.kafkaSection);
            string brokerURL = kafkaSection[ConfigurationKeys.kafka_broker1];
            string topicPrimary = kafkaSection[ConfigurationKeys.kafka_assetTopicPrimary];
            string topicSecondary = kafkaSection[ConfigurationKeys.kafka_assetTopicSecondary];
            string groupSecondary = kafkaSection[ConfigurationKeys.kafka_assetGroupSecondary];
            string groupPrimary = kafkaSection[ConfigurationKeys.kafka_assetGroupPrimary];
            var clamHost = Configuration.GetSection(ConfigurationKeys.clamAV_Section)[ConfigurationKeys.clamAV_Host];
            services.AddSingleton<ClamClient>(new ClamClient(clamHost));

            services.AddSingleton<KafkaProducer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<KafkaProducer>>();
                var kafkaConfig = new KafkaConfiguration(topicPrimary, brokerURL, groupPrimary, "ProductAPI", topicSecondary, groupSecondary);
                var asynckafkaProducer = KafkaProducer.BuildProducer(logger, kafkaConfig);
                var kafkaProducer = asynckafkaProducer.GetAwaiter().GetResult();
                return kafkaProducer;
            });
        }
    }
}