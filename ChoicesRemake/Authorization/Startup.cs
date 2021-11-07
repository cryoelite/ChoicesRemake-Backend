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

namespace Authorization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connStr = Configuration.GetSection("Authorization")["connectionString"];
            services.AddDbContext<AuthorizationDBContext>(o => o.UseSqlServer(connStr));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authorization", Version = "v1" });
            });
            var kafkaSection = Configuration.GetSection(KafkaConfiguration.KafkaName);
            string brokerURL = kafkaSection["broker1"];
            string topic = kafkaSection["topic1"];
            string group = kafkaSection["group1"];
            
            services.AddSingleton((_) =>
            {
                var dbContextOptions = new DbContextOptionsBuilder<AuthorizationDBContext>();
                dbContextOptions.UseSqlServer(connStr);

                return dbContextOptions.Options;
            });
            services.AddSingleton<Consumer>();
            services.AddHostedService<KafkaConsumer>(sp=> {
                var consumer = sp.GetRequiredService<Consumer>();
                var config = new KafkaConfiguration(topic, brokerURL, group, "Authorization");
                var logger = sp.GetRequiredService<ILogger<KafkaConsumer>>();
                return new KafkaConsumer(logger, config, consumer.ManageMessage);
            
            });
        }

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
    }
}