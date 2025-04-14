using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MilkTeaMe.Repositories.DbContexts;
using MilkTeaMe.Repositories.Implementations;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Implementations;
using MilkTeaMe.Services.Interfaces;
using System.Configuration;

namespace MilkTeaMe.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MilkTeaMeDBContext>(options =>
				 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.Configure<VNPaySettings>(builder.Configuration.GetSection("VNPaySettings"));

			builder.Services.AddScoped(typeof(GenericRepository<>));
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<IDashboardService, DashboardService>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IVNPayService, VNPayService>();
			builder.Services.AddScoped<IPaymentService, PaymentService>();
			builder.Services.AddScoped<CloudinaryService>();

             builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                 option =>
                 {
                     option.LoginPath = "/Auth/Login";
                     option.AccessDeniedPath = "/Auth/AccessDenied";
                     option.ExpireTimeSpan = TimeSpan.FromDays(7);
                     option.SlidingExpiration = true;
                     option.Cookie.SameSite = SameSiteMode.Lax;
                     option.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                 });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller}/{action}");

            //app.MapAreaControllerRoute(
            //    name: "Manager",
            //    areaName: "Manager",
            //    pattern: "Manager/{controller=Dashboard}/{action=Index}/{id?}");

            //app.MapAreaControllerRoute(
            //    name: "Customer",
            //    areaName: "Customer",
            //    pattern: "Customer/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
