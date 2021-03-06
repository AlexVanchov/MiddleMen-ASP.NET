﻿namespace MiddleMan.Web
{
    using System;
    using System.IO;
    using System.Reflection;

    using AutoMapper;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MiddleMan.Data;
    using MiddleMan.Data.Common;
    using MiddleMan.Data.Common.Repositories;
    using MiddleMan.Data.Models;
    using MiddleMan.Data.Repositories;
    using MiddleMan.Data.Seeding;
    using MiddleMan.Services;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Services.Messaging;
    using MiddleMan.Services.Services;
    using MiddleMan.Web.Controllers;
    using MiddleMan.Web.Hubs;
    using MiddleMan.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(@"D:\HostingSpaces\alexgta1500\alexvanchov.uk\wwwroot\"));

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            Account cloudinaryCredentials = new Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryCredentials);

            services.AddSingleton(cloudinaryUtility);

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default User settings.
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = this.configuration["Facebook:AppId"];
                    facebookOptions.AppSecret = this.configuration["Facebook:AppSecret"];
                })
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                    this.configuration.GetSection("Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });

            services.AddSignalR();
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();
            // services.AddScoped<ICloudinaryService, CloudinaryService>();

            // Application services
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<IOrderService, OrderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var options = new RewriteOptions()
            .AddRedirectToHttpsPermanent();

            app.UseRewriter(options);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("/error/{0}");

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<MessageHub>("/details");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
