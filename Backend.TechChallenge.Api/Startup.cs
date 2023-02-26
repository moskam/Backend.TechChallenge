using Backend.TechChallenge.Api.Configuration;
using Backend.TechChallenge.Api.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Backend.TechChallenge.Api;

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
        services.AddMemoryCache();

        services.AddControllers()
            .AddFluentValidation(x =>
            {
                x.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

        services.AddSwaggerGen();

        services.ConfigureAppSettings(Configuration)
            .ConfigureLogging(Configuration)
            .AddUsersFeature();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
        app.UseRouting();

        app.UseAuthorization();

        app.UseMiddleware<ErrorHandlerMiddleware>(); // global error handler

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
