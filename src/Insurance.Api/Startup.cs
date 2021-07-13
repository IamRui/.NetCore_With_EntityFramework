using Insurance.Api.Extensions;
using Insurance.Data.Access.DBContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Insurance.Api
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
	        // Register the Swagger generator, defining 1 or more Swagger documents
	        services.AddSwaggerGen(c =>
	        {
		        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Insurance API", Version = "v1" });
	        });

			//Register db
			services.AddDbContext<InsuranceContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //AddLogging
			services.AddLogging(config => { config.AddConsole(); config.AddDebug(); })
				.Configure<LoggerFilterOptions>(config => config.MinLevel = LogLevel.Debug);

	        services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

			services.ResolveDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
	        if (env.IsDevelopment())
	        {
		        app.UseDeveloperExceptionPage();
	        }
	        else
	        {
		        app.UseExceptionHandler(appBuilder =>
		        {
			        appBuilder.Run(async c => {
				        c.Response.StatusCode = 500;
				        await c.Response.WriteAsync($"Something happened. Please try again later!");
			        });
		        });
	        }

            //Create db
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
			{
				var context = serviceScope?.ServiceProvider.GetRequiredService<InsuranceContext>();
				context?.Database.EnsureCreated();
			}

			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
	            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance API");
            });
        }
    }
}
