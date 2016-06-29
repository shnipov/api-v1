using System;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.SwaggerGen.Generator;
using TodoApi.Models.RussianPost;

namespace TodoApi
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTokenAuthorization();

            services
                .AddIdentity<CustomUser, Role>(options =>
                {

                })
                .AddUserStore<CustomUserStore>()
                .AddUserManager<CustomUserManager>()
                .AddRoleStore<CustomRoleStore>()
                .AddRoleManager<CustomRoleManager>();

            // Настройки ISwaggerProvider документации API
            services.AddSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Description = "Работа с доставкой",
                    TermsOfService = "-",
                    Title = "Delivery API"
                });

                options.OperationFilter<TokenAuthorizationOperationFilter>();

                options.DescribeAllEnumsAsStrings();

                string xmlDocPath = Path.Combine(_env.ContentRootPath,  @"bin\Debug\netcoreapp1.0\TodoApi.xml");
                if (File.Exists(xmlDocPath))
                {
                    options.IncludeXmlComments(xmlDocPath);
                }
            });

            //services.AddScoped<IOptions<IdentityOptions>>(provider =>
            //{
            //    return new OptionsManager<IdentityOptions>(new []{new ConfigureOptions<IdentityOptions>(o => { o.Cookies. }) });
            //});

            services.AddScoped<IRussianPostLogic, RussianPostLogic>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            RsaSecurityKey key,
            TokenAuthOptions tokenOptions)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    //// Basic settings - signing key to validate with, audience and issuer.
                    IssuerSigningKey = key,
                    ValidAudience = tokenOptions.Audience,
                    ValidIssuer = tokenOptions.Issuer,

                    // When receiving a token, check that we've signed it.
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    //ValidateSignature = true,

                    // When receiving a token, check that it is still valid.
                    ValidateLifetime = true,

                    // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                    // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                    // machines which should have synchronised time, this can be set to zero. Where external tokens are
                    // used, some leeway here could be useful.
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            });

            app.UseIdentity();

            app.UseMvc();
            
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwaggerGen(routeTemplate: "doc/{apiVersion}/info.json");

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi(baseRoute: "doc", swaggerUrl: "/doc/v1/info.json");
        }
    }
}
