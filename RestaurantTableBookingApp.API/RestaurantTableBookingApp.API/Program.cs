
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RestaurantTableBookingApp.Data;
using RestaurantTableBookingApp.Service;
using Serilog;
using System.Net;

namespace RestaurantTableBookingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog with the settings
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .CreateBootstrapLogger();
			try
			{
                var builder = WebApplication.CreateBuilder(args);
                var configuration = builder.Configuration;

                builder.Services.AddApplicationInsightsTelemetry();

                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration.WriteTo.ApplicationInsights(
                    services.GetRequiredService<TelemetryConfiguration>(),
                    TelemetryConverter.Events));

                Log.Information("Startingthe application...");

                // Add services to the container.

                builder.Services.AddDbContext<RestaurantTableBookingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbContext") ?? "")
                .EnableSensitiveDataLogging() //should not be usedd in production, only in dev
                );


                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
                builder.Services.AddScoped<IRestaurantService, RestaurantService>();

                var app = builder.Build();

                //Exception handling, Create a middleware and include that here
                // Enable Serilog exception logging
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception,"Unhandled exception occured. {ExceptionDetails}", exception?.ToString());

                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occured. Please try again.");
                    });
                });


                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
			catch (Exception)
			{

				throw;
			}
        }
    }
}
