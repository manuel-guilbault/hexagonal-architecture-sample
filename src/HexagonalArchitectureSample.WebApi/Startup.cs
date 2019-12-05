using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using HexagonalArchitectureSample.Domain;
using HexagonalArchitectureSample.Infrastructure;
using HexagonalArchitectureSample.UseCases;
using HexagonalArchitectureSample.UseCases.BookSeats;
using HexagonalArchitectureSample.UseCases.CreateFlight;
using HexagonalArchitectureSample.UseCases.GetFlightDetails;
using HexagonalArchitectureSample.UseCases.ListFlights;
using HexagonalArchitectureSample.UseCases.UpdateFlightCapacity;
using HexagonalArchitectureSample.WebApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace HexagonalArchitectureSample.WebApi
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
            services.AddControllers(o =>
            {
                o.Filters.Add(new MapExceptionToHttpStatusCodeFilterAttribute(
                    typeof(DBConcurrencyException),
                    StatusCodes.Status503ServiceUnavailable));
            });

            services.AddAutoMapper(GetType().Assembly);

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo() { Title = "HexagonalArchitectureSample", Version = "v1" });
            });

            services
                .AddScoped(_ => new SqlConnection(Configuration["DbConnectionString"]))
                .AddSingleton(new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                    {
                        new ValueObjectJsonConverter<FlightId>(
                            x => new FlightId(Guid.Parse(x)),
                            x => x.Value.ToString()),
                        new ValueObjectJsonConverter<BookingId>(
                            x => new BookingId(Guid.Parse(x)),
                            x => x.Value.ToString()),
                    },
                })
                .AddTransient<IRepository<FlightId, Flight>, FlightRepository>()
                .AddTransient<IQueryHandler<ListFlightsQuery, IEnumerable<FlightSummary>>, ListFlightQueryHandler>()
                .AddTransient<IQueryHandler<GetFlightDetailsQuery, FlightDetails?>, GetFlightDetailsQueryHandler>()
                .AddTransient<ICommandHandler<CreateFlightCommand, CreateFlightResult>, CreateFlightCommandHandler>()
                .AddTransient<ICommandHandler<UpdateFlightCapacityCommand, UpdateFlightCapacityResult>, UpdateFlightCapacityCommandHandler>()
                .AddTransient<ICommandHandler<BookSeatsCommand, BookSeatsResult>, BookSeatsCommandHandler>()
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "Hexagonal Architecture Sample (v1)");
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
