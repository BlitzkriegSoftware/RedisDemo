using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

namespace Blitz.Redis.WebDemo
{
    /// <summary>
    /// Start up
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// API Version
        /// </summary>
        public const string ApiVersion = "v1";

        /// <summary>
        /// Configuration Property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddLogging(loggingBuilder => {
                // This line must be 1st
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);

                // Console is generically cloud friendly
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            _ = services.AddCors(cors =>
            {
                cors.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                });
            });

            _ = services.AddMvc(
                config =>
                {
                    config.Filters.Add(typeof(Libs.GlobalExceptionFilter));
                }
            );

            _ = services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "SampleInstance";
            });

            _ = services.AddControllers();

            _ = services.AddSwaggerGen(swag =>
            {

                swag.SwaggerDoc(ApiVersion,
                    new OpenApiInfo()
                    {
                        Contact = new OpenApiContact()
                        {
                            Email = "spookdejur@hotmail.com",
                            Name = "Stuart Williams",
                            Url = new Uri("https://github.com/BlitzkriegSoftware/RedisDemo")
                        },
                        Description = Program.ProgramMetadata.Description,
                        License = new OpenApiLicense()
                        {
                            Name = "MIT",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        },
                        Title = Program.ProgramMetadata.Description,
                        Version = Program.ProgramMetadata.SemanticVersion
                    });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (System.IO.File.Exists(xmlPath)) swag.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IWebHostEnvironment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var imgPath = env.WebRootPath + "/assets/images/favicon-32x32.png";

            app.UseSwagger();

            app.UseSwaggerUI(ui =>
            {
                ui.HeadContent = "<link rel=\"icon\" type=\"image/png\" href=\"" + imgPath + "\" sizes=\"32x32\" />";
                ui.InjectStylesheet("/assets/css/Override.css");
                ui.InjectJavascript("/assets/js/AddLogo.js");

                ui.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", $"{Program.ProgramMetadata.Description} {ApiVersion}");

                ui.ShowExtensions();
                ui.DisplayRequestDuration();
                ui.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
