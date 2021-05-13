using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataBase;
using Infrastructure.Helpers;
using Infrastructure.Mapping;
using Infrastructure.Options;
using Infrastructure.Services.AdService;
using Infrastructure.Services.DataProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AdsApp.API
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
            var assembly = typeof(AutoMapping).Assembly;
            services.AddAutoMapper(assembly);
            
            services.Configure<StaticFilesOptions>(Configuration.GetSection(nameof(StaticFilesOptions)));
            services.Configure<AuthOptions>(Configuration.GetSection(nameof(AuthOptions)));
            services.Configure<UserOptions>(Configuration.GetSection(nameof(UserOptions)));
            
            services.AddScoped<IAdService, AdService>();
            services.AddScoped<IDataProvider, DataProvider>();
            services.AddScoped<IImageHelper, ImageHelper>();
            
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile($"appsettings.json").Build();
            var connection = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<AdsAppContext>(options => options.UseNpgsql(connection));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "AdsApp.API", Version = "v1"});
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdsApp.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}