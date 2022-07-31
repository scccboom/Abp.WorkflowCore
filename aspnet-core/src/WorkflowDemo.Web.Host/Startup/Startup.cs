using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Json;

using Castle.Facilities.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

using WorkflowDemo.Configuration;
using WorkflowDemo.Identity;
using WorkflowDemo.Workflows;

namespace WorkflowDemo.Web.Host.Startup
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private const string _apiVersion = "v1";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            _hostingEnvironment = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public void ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
                }
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddSignalR();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            ConfigureSwagger(services);

            #region WorkflowCore

            services.AddWorkflow(options =>
            {
                options.UsePersistence(sp =>
                {
                    return sp.GetService<AbpPersistenceProvider>();
                });
            });
            services.AddWorkflowDSL();
            //services.AddWrokflowHostedService();

            #endregion WorkflowCore

            // Configure Abp and Dependency Injection
            services.AddAbpWithoutCreatingServiceProvider<WorkflowDemoWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                        ? "log4net.config"
                        : "log4net.Production.config"
                    )
                )
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<HttpLoggingMiddleware>();

            // Initializes ABP framework.
            app.UseAbp(options => 
            { 
                options.UseAbpRequestLocalization = false; 
            });

            app.UseWorkflow(true);

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAbpRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "";
                // specifying the Swagger JSON endpoint.
                options.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/{_apiVersion}/swagger.json", $"WorkflowDemo API {_apiVersion}");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("WorkflowDemo.Web.Host.wwwroot.swagger.ui.index.html");
                options.DisplayRequestDuration(); // Controls the display of the request duration (in milliseconds) for "Try it out" requests.
            }); // URL: /swagger
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(_apiVersion, new OpenApiInfo
                {
                    Version = _apiVersion,
                    Title = "WorkflowDemo API",
                    Description = "WorkflowDemo",
                    // uncomment if needed TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "WorkflowDemo",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/aspboilerplate"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/LICENSE"),
                    }
                });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                //add summaries to swagger
                bool canShowSummaries = _appConfiguration.GetValue<bool>("Swagger:ShowSummaries");
                if (canShowSummaries)
                {
                    var hostXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var hostXmlPath = Path.Combine(AppContext.BaseDirectory, hostXmlFile);
                    options.IncludeXmlComments(hostXmlPath);

                    var applicationXml = $"WorkflowDemo.Application.xml";
                    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXml);
                    options.IncludeXmlComments(applicationXmlPath);

                    var webCoreXmlFile = $"WorkflowDemo.Web.Core.xml";
                    var webCoreXmlPath = Path.Combine(AppContext.BaseDirectory, webCoreXmlFile);
                    options.IncludeXmlComments(webCoreXmlPath);
                }
            });
        }
    }
}