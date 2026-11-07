using CinemaTicketBooking.Utitlites;
using Microsoft.AspNetCore.Identity;

namespace CinemaTicketBooking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                              ?? throw new InvalidOperationException("Connection string"
                                            + "'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IReposetory<Movie>, Reposetory<Movie>>();
            builder.Services.AddScoped<IReposetory<Actor>, Reposetory<Actor>>();
            builder.Services.AddScoped<IReposetory<Cinema>, Reposetory<Cinema>>();
            builder.Services.AddScoped<IReposetory<Category>, Reposetory<Category>>();
            builder.Services.AddScoped<IReposetory<MovieActor>, Reposetory<MovieActor>>();
            builder.Services.AddScoped<IReposetory<MovieSubImage>, Reposetory<MovieSubImage>>();
            builder.Services.AddScoped<IReposetory<ApplicationUserOTP>, Reposetory<ApplicationUserOTP>>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Lockout.MaxFailedAccessAttempts = 4;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();



            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Identity}/{controller=Account}/{action=Register}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
