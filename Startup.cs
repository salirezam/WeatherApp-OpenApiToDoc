using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;

namespace RESTAPI_SwaggerToMarkup
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Weather App Documentation",
                        Version = "v1",
                        Description = File.ReadAllText("README.md"),
                        Extensions = new Dictionary<string, IOpenApiExtension>()
                        { 
                            ["x-logo"] = new OpenApiObject
                            {
                                ["url"] = new OpenApiString("https://upload.wikimedia.org/wikipedia/en/thumb/b/b1/NewDay_logo.png/220px-NewDay_logo.png"),
                                ["backgroundColor"] = new OpenApiString("#FFFFFF"),
                                ["altText"] = new OpenApiString("NewDay Logo")
                            }
                        }
                    });
                c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));

                c.TagActionsBy(api => new[] {api.GroupName});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RESTAPI_SwaggerToMarkup v1"));
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
