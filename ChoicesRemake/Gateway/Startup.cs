using Gateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using StaticAssets;
using System;
using System.Security.Claims;
using System.Text;

namespace Gateway
{
    public class Startup
    {
        public const string authKey = "AuthKey";

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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway v1"));
            }

/*            app.UseHttpsRedirection();*/

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var configuration = new OcelotPipelineConfiguration
            {
                AuthorizationMiddleware = async (httpContext, next) =>
                {
                    await OcelotAuthorizationMiddleware.Authorize(httpContext, next);
                }
            };
            app.UseOcelot(configuration).Wait();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = new JWTSettings()
            {
                DataEncryptionAlgorithm = Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_DataEncryptionAlgorithm),
                EncryptionKey= Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_EncryptionKey),
                ExpirationInDays= Configuration.GetValue<int>(ConfigurationKeys.authentication_jwt_expiration),
                Issuer= Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_issuer),
                KeyWrapAlgorithm= Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_KeyWrapAlgorithm),
                SigningAlgorithm= Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_SigningAlgorithm),
                SigningKey= Configuration.GetValue<string>(ConfigurationKeys.authentication_jwt_SigningKey),
            };
            services.AddSingleton((_)=>jwtSettings);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("JwtBased", policy =>
                {
                    policy.RequireClaim(claimType: ClaimTypes.Role);
                });
            }).AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(authKey, o =>
             {
                 var signingKey = Encoding.UTF8.GetBytes(jwtSettings.SigningKey);
                 var encKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptionKey);
                 var byteArray = new byte[32];
                 Array.Copy(encKey, byteArray, 32);

                 var encSigningKey = new SymmetricSecurityKey(signingKey);
                 var encEncKey = new SymmetricSecurityKey(byteArray);

                 var validationParams = new TokenValidationParameters()
                 { TokenDecryptionKey = encEncKey, IssuerSigningKey = encSigningKey, ValidAudience = jwtSettings.Issuer, ValidIssuer = jwtSettings.Issuer };

                 o.TokenValidationParameters = validationParams;
                 /*new TokenValidationParameters
                 {
                     ValidIssuer = jwtSettings.Issuer,
                     ValidAudience = jwtSettings.Issuer,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
                     TokenDecryptionKey = new SymmetricSecurityKey(byteArray),
                     ClockSkew = TimeSpan.Zero,
                     ValidAlgorithms = new[] { jwtSettings.DataEncryptionAlgorithm, jwtSettings.KeyWrapAlgorithm, jwtSettings.SigningAlgorithm },
                 };*/
             });
            services.AddOcelot();
        }
    }
}