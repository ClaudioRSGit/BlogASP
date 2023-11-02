using BlogASP;
using BlogASP.DAL;
using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

//////////////////Login///////////////////

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; 
        options.AccessDeniedPath = "/Home/AccessDenied"; 
    });

builder.Services.AddAuthorization();

//////////////////Login///////////////////

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuration of the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

//////////////////////////////// Admin Default
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<DatabaseContext>();
    var userRepository = services.GetRequiredService<IUserRepository>();

    var existingAdmin = userRepository.GetUserByUsername("Administrator");

    if (existingAdmin == null)
    {
        var adminUser = new UserModel
        {
            Name = "Administrator",
            Username = "Administrator",
            Email = "administrator@admin.com"
            // Defina a senha e outras propriedades aqui
        };

        string adminPassword = "Admin123!"; 
        adminUser.Password = PasswordHasher.HashPassword(adminPassword);

        adminUser.Role = "Administrator"; 

        dbContext.Users.Add(adminUser);
        dbContext.SaveChanges();
    }
}
////////////////////////////////////////////////

app.Run();