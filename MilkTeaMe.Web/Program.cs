using Microsoft.EntityFrameworkCore;
using MilkTeaMe.Repositories.DbContexts;
using MilkTeaMe.Repositories.Implementations;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.Implementations;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Web.Infrastructure.ViewLocationExpanders;

namespace MilkTeaMe.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorOptions(options =>{
				options.ViewLocationExpanders.Add(new ManagerViewLocationExpander());
			});

			builder.Services.AddDbContext<MilkTeaMeDBContext>(options =>
				 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped(typeof(GenericRepository<>));
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IEmployeeService, EmployeeService>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<CloudinaryService>();

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

			app.Use(async (context, next) =>
			{
				var routeData = context.GetRouteData();
				if (routeData != null)
				{
					Console.WriteLine($"Controller: {routeData.Values["controller"]}, Action: {routeData.Values["action"]}");
				}
				await next();
			});


			app.UseAuthorization();

			app.MapControllerRoute(
				name: "Manager",
				pattern: "Manager/{controller=Dashboard}/{action=Index}");

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
        }
    }
}
