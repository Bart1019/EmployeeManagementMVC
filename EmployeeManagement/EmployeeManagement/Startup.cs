using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure;
using EmployeeManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeeManagement
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<EmployeeDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("EmployeeDbConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 5;

                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<EmployeeDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1); 
            });

            services.AddControllersWithViews();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));

            }).AddXmlSerializerFormatters();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "89150711082-70757ndcugsku8gq4gslv43rjjbg2isp.apps.googleusercontent.com";
                    options.ClientSecret = "4oO1wIJlUsYl6AQEiPKiA5wT";
                })
                .AddFacebook(options =>
                {
                    options.AppId = "807269850193908";
                    options.AppSecret = "35affa720c10ae2fb1564b2240ad556a";
                });
                

            services.AddAuthorization(options => options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role")));
            services.AddAuthorization(options => options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role")));
            services.AddAuthorization(options => options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role")));

            services.AddAuthorization(options => options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin", "User")));

            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

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
                app.UseStatusCodePagesWithRedirects("/Home/Error");

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
