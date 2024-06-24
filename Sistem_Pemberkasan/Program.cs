using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sistem_Pemberkasan.Models.EF;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ModelContext>(options =>
        options.UseMySql("server=10.21.39.88;port=3306;database=sistem_pemberkasan;user=spb_mgg;password=Bapenda.2024",
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"))
        );
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // Redirect to Login action on unauthorized access
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/AccessDenied"; // Redirect on forbidden actions
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set cookie expiration
        
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Pengelola",
         policy => policy.RequireRole("Pengelola"));
    options.AddPolicy("Perekam",
         policy => policy.RequireRole("Perekam"));
    options.AddPolicy("User",
         policy => policy.RequireRole("User"));
});


builder.Services.Configure<PasswordHasherOptions>(option =>
{
    option.IterationCount = 12000;
});

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".SisBer_SESSION_COOKIE"; // Customize cookie name
    options.IdleTimeout = TimeSpan.FromMinutes(300); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

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

app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
