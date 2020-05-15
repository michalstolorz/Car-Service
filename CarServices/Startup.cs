using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarServices.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CarServices
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            //Configuration = configuration;
            _config = config;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("CarServicesDBConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 3;
            }).AddEntityFrameworkStores<AppDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvc().AddXmlSerializerFormatters();

            services.AddScoped<ICarBrandRepository, SQLCarBrandRepository>();
            services.AddScoped<ICarModelRepository, SQLCarModelRepository>();
            services.AddScoped<ICarRepository, SQLCarRepository>();
            services.AddScoped<ICustomerRepository, SQLCustomerRepository>();
            services.AddScoped<IEmployeesRepository, SQLEmployeesRepository>();
            services.AddScoped<IInvoiceRepository, SQLInvoiceRepository>();
            services.AddScoped<IOrderRepository, SQLOrderRepository>();
            services.AddScoped<IPartsRepository, SQLPartsRepository>();
            services.AddScoped<IRepairRepository, SQLRepairRepository>();
            services.AddScoped<IRepairTypeRepository, SQLRepairTypeRepository>();
            services.AddScoped<IUsedPartsRepository, SQLUsedPartsRepository>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
