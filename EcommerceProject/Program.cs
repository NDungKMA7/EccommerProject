using EcommerceProject.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EcommerceProject.Models.Mapping;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectString")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddRoleManager<RoleManager<IdentityRole>>().AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddAuthorization(options =>
 {
     options.AddPolicy("Admin", policy =>
     {
         policy.RequireRole("Admin");
     });
     options.AddPolicy("Author", policy =>
     {
         policy.RequireRole("Admin", "Author");
     });
     options.AddPolicy("Seller", policy =>
     {
         policy.RequireRole("Admin", "Seller");
     });
 });
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Customer", "Author", "Seller", };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.UseSession();

app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "area", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
