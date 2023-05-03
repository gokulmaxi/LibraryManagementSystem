using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Areas.Identity.Data;
namespace LibraryManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("LibraryManagementSystemContextConnection") ?? throw new InvalidOperationException("Connection string 'LibraryManagementSystemContextConnection' not found.");

            builder.Services.AddDbContext<LibraryManagementSystemContext>(options =>
options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<LMSUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<LibraryManagementSystemContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.MapRazorPages();
            app.UseRouting();
            app.UseAuthentication(); ;

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            //seeding user roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Manger" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            //seeding admin user
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<LMSUser>>();
                string mail = "admin@presidio.com";
                string pass = "Verystr0ngpass@";
                if (await userManager.FindByEmailAsync(mail) == null)
                {
                    Console.WriteLine("seeding admin user");
                    var user = new LMSUser();
                    user.Email = mail;
                    user.UserName = mail;
                    await userManager.CreateAsync(user, pass);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            app.Run();
        }
    }
}