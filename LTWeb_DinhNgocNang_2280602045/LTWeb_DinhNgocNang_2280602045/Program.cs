using LTWeb_DinhNgocNang_2280602045.Models;
using LTWeb_DinhNgocNang_2280602045.Repositories;
using LTWeb_DinhNgocNang_2280602045.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình các dịch vụ
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ShoppingCartService>();
// ✅ BỎ CHẶN PHÂN QUYỀN TOÀN APP
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAssertion(_ => true)
        .Build();
});
// REST API + Swagger
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Middleware

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseStatusCodePagesWithReExecute("/ProductDisplay/AccessDenied");

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Response.Redirect("/Identity/Account/Login");
            return;
        }

        if (!context.User.IsInRole("Admin"))
        {
            context.Response.Redirect("/ProductDisplay/AccessDenied");
            return;
        }
    }

    await next();
});


// ✅ Swagger
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Định tuyến
app.UseEndpoints(endpoints =>
{
    // REST API
    endpoints.MapControllers();

    // Razor Pages (Identity)
    endpoints.MapRazorPages();

    // ShoppingCart
    endpoints.MapControllerRoute(
        name: "shoppingCart",
        pattern: "ShoppingCart/{action=Index}/{id?}",
        defaults: new { controller = "ShoppingCart" });

    // Admin
    endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");

    // Mặc định
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=ProductDisplay}/{action=Index}/{id?}");
});

// 3. Khởi tạo quyền và admin user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    string[] roleNames = { "Admin", "Customer", "Employee" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminEmail = "admin@example.com";
    var adminPassword = "Admin@123";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FullName = "Admin User",
            PhoneNumber = "0123456789"
        };
        var result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
    else if (!await userManager.IsInRoleAsync(admin, "Admin"))
    {
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

// Đường dẫn POST xóa session tạm
app.MapPost("/Identity/Account/ClearTempData", async (context) =>
{
    context.Session.Remove("LogoutSuccess");
    await Task.CompletedTask;
});

app.Run();
