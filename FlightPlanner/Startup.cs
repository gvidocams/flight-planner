using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Core.Validations;
using FlightPlanner.Data;
using FlightPlanner.Filters;
using FlightPlanner.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FlightPlanner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlightPlanner", Version = "v1" });
            });
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthorizationHandler>("BasicAuthentication", null);
            
            var connectionString = Configuration.GetConnectionString("Flight-planner");
            
            services.AddDbContext<FlightPlannerDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
            services.AddScoped<IFlightPlannerDbContext, FlightPlannerDbContext>();
            services.AddScoped<IDbService, DbService>();
            services.AddScoped<IEntityService<Airport>, EntityService<Airport>>();
            services.AddScoped<IEntityService<Flight>, EntityService<Flight>>();
            services.AddScoped<IEntityService<User>, EntityService<User>>();
            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<IFlightValidator, CarrierValidator>();
            services.AddScoped<IFlightValidator, FlightTimeValidator>();
            services.AddScoped<IFlightValidator, FlightAirportValidator>();
            services.AddScoped<IAirportValidator, AirportCountryValidator>();
            services.AddScoped<IAirportValidator, AirportCityValidator>();
            services.AddScoped<IAirportValidator, AirportCodeValidator>();
            services.AddSingleton<IMapper>(AutoMapperConfig.CreateMapper());
        }

        

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlightPlanner v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
