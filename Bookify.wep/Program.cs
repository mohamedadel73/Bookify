using Bookify.wep.Data;
using Bookify.Data.Data;
using Bookify.service.Repositories;
using Bookify.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;

// Alias DbContexts to avoid ambiguity
using IdentityDbContext = Bookify.wep.Data.ApplicationDbContext;
using HotelDbContext = Bookify.Data.Data.ApplicationDbContext;

namespace Bookify.wep
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Register Hotel DbContext for domain data access
            var hotelConnection = builder.Configuration.GetConnectionString("HotelConnection") ?? throw new InvalidOperationException("Connection string 'HotelConnection' not found.");
            builder.Services.AddDbContext<HotelDbContext>(options =>
                options.UseSqlServer(hotelConnection));

            // Repositories
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IGuestRepository, GuestRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdentityDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
