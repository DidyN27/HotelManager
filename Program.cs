using Hotel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Hotel.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Свързване с базата данни
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// 2. ДОБАВЯНЕ НА IDENTITY (Това липсваше!)
// Тук казваме на системата да използва ApplicationUser и да добави поддръжка за Razor Pages (Identity страниците)
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>() 
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Нужно е за Identity страниците като Login/Register

var app = builder.Build();

// 3. Конфигурация на Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ВАЖНО: Първо Authentication (кой си ти), после Authorization (какво можеш да правиш)
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Нужно е, за да работят Login/Register страниците
app.MapRazorPages(); 

app.Run();