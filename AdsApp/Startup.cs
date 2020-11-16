using AdsApp.MiddleWares;
using AutoMapper;
using DataBase;
using DTO.AdRequest;
using DTO.Request;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Helpers;
using Infrastructure.Helpers.TokenHelper;
using Infrastructure.Options;
using Infrastructure.Services.AdService;
using Infrastructure.Services.DataProvider;
using Infrastructure.Services.UserService;
using Infrastructure.Services.Validations;
using Infrastructure.Validations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AdsApp
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
            services.Configure<StaticFilesOptions>(Configuration.GetSection(nameof(StaticFilesOptions)));
            services.Configure<AuthOptions>(Configuration.GetSection(nameof(AuthOptions)));
            services.Configure<UserOptions>(Configuration.GetSection(nameof(UserOptions)));
            services.AddAutoMapper(typeof(Startup));

            string connection = Configuration.GetConnectionString("DefaultConnection");
            var authOptions = new AuthOptions();
            Configuration.Bind("AuthOptions", authOptions);
            services.AddDbContext<AdsAppContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();
            
            services.AddMvc().AddFluentValidation();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = authOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
                options.LoginPath = "/User/Login";
                options.Cookie.Name = "access_token";
                options.Cookie.HttpOnly = true;
                options.TicketDataFormat = new JwtDataFormat(SecurityAlgorithms.HmacSha256, tokenValidationParameters);

            });

            services.AddScoped<IDataProvider, DataProvider>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdService, AdService>();
            services.AddTransient<IImageHelper, ImageHelper>();
            services.AddTransient<ITokenHelper, TokenHelper>();
            services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddTransient<IValidator<AdvertisementRequest>, AdValidator>();
            services.AddTransient<IValidator<AddAdvertisementRequest>, AddAdvertisementValidator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<ImageMiddleWare>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Ads}/{action=Index}/{id?}");
            });
        }
    }
}