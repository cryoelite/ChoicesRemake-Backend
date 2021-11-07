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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JWTSettings>(Configuration.GetSection("Authentication").GetSection("Jwt"));
            var jwtSettings = Configuration.GetSection("Authentication").GetSection("Jwt").Get<JWTSettings>();
            var connStr = Configuration.GetSection("Authentication")["connectionString"];
       
            services.AddDbContext<ApplicationDBContext>(o => o.UseSqlServer(connStr));

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ClockSkew = TimeSpan.Zero,
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
            });
            var kafkaSection = Configuration.GetSection(KafkaConfiguration.KafkaName);
            string brokerURL = kafkaSection["broker1"];
            string topic = kafkaSection["topic1"];
            string group = kafkaSection["group1"];

            services.AddSingleton<KafkaProducer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<KafkaProducer>>();
                var kafkaConfig = new KafkaConfiguration(topic, brokerURL, group, "Authentication");
                var asynckafkaProducer = KafkaProducer.BuildProducer(logger, kafkaConfig);
                var kafkaProducer = asynckafkaProducer.GetAwaiter().GetResult();
                return kafkaProducer;
            });

            var obj = services.BuildServiceProvider();
            using (var scope = obj.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                logger.LogInformation($"The connection string is {connStr}");
            
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}