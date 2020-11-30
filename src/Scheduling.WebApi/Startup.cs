using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scheduling.Business;
using Scheduling.Data.Sql;
using Scheduling.Data.Sql.Repositories;
using Scheduling.Infrastructure.Business;
using Scheduling.Infrastructure.Data;
using Scheduling.WebApi.Middleware;

namespace Scheduling.WebApi
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
            
            services.AddDbContext<SchedulingDbContext>(b => b.UseInMemoryDatabase("Scheduling"), ServiceLifetime.Scoped);

            services.AddScoped<ITimeSlotBusinessLogic, TimeSlotBusinessLogic>();
            services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<BasicExceptionMiddleware>();

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
