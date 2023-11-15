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

//Login

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; 
        options.AccessDeniedPath = "/Home/AccessDenied"; 
    });

builder.Services.AddAuthorization();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configuration of the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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


//Admin Default
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<DatabaseContext>();
    var userRepository = services.GetRequiredService<IUserRepository>();

    var existingAdmin = userRepository.GetUserByUsername("Administrator");
    dbContext.Database.EnsureCreated();

    if (existingAdmin == null)
    {
        var adminUser = new UserModel
        {
            Name = "Administrator",
            Username = "Administrator",
            Email = "administrator@admin.com"
        };

        string adminPassword = "Admin123!"; 
        adminUser.Password = PasswordHasher.HashPassword(adminPassword);

        adminUser.Role = "Administrator"; 

        dbContext.Users.Add(adminUser);
        dbContext.SaveChanges();
    }

}


//standard articles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<DatabaseContext>();
    var articleRepository = services.GetRequiredService<IArticleRepository>();

    dbContext.Database.EnsureCreated();

    // Check if articles already exist (optional)
    if (!dbContext.Articles.Any())
    {
        var article1 = new ArticleModel
        {
            Title = "Machine Learning Applications in Healthcare",
            Description = "Exploring the transformative impact of machine learning in healthcare, from diagnostics to personalized treatment.",
            Category = "Data Science",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            UserName = "bySeeder",
            isDisabled = false,
            Stars = 0,
            Picture = "https://previews.123rf.com/images/olegdudko/olegdudko1908/olegdudko190803202/128612103-big-data-analytics-and-business-intelligence-concept-with-chart-and-graph-icons-on-a-digital-screen.jpg?fj=1",
        };
        dbContext.Articles.Add(article1);

        var article2 = new ArticleModel
        {
            Title = "The Rise of DevOps: Bridging the Gap Between Development and Operations",
            Description = "Introduction: Explore the evolution of DevOps and how it has transformed the collaboration between development and operations teams.\r\nKey Points:\r\nThe principles of DevOps and its impact on software development.\r\nBenefits of implementing DevOps practices in organizations.\r\nReal-world examples of successful DevOps adoption.",
            Category = "DevOps",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            UserName = "bySeeder",
            Stars = 2,
            isDisabled = false,
            isPrivate = true,
            Picture = "https://wallpaperbat.com/img/873635-devops-testing-services-agile-testing.jpg",
        };
        dbContext.Articles.Add(article2);

        var article3 = new ArticleModel
        {
            Title = "Web Development Trends in 2023: A Comprehensive Overview",
            Description = "Introduction: Dive into the latest trends shaping the landscape of web development in 2023.\r\nKey Points:\r\nEmerging technologies influencing web development.\r\nThe role of AI, blockchain, and other cutting-edge technologies.\r\nHow developers can stay ahead by adapting to these trends.",
            Category = "Web Development",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            UserName = "bySeeder",
            Stars = 5,
            isDisabled = false,
            isPrivate = true,
            Picture = "https://e0.pxfuel.com/wallpapers/185/259/desktop-wallpaper-front-end-web-developer-vacancy-egeniq.jpg",
        };
        dbContext.Articles.Add(article3);

        var article4 = new ArticleModel
        {
            Title = "Mastering Server Security: Best Practices for Ensuring a Robust Infrastructure",
            Description = "Introduction: Address the critical aspects of server security and the importance of safeguarding digital assets.\r\nKey Points:\r\nCommon security threats faced by servers.\r\nBest practices for securing servers, including encryption and regular updates.\r\nCase studies highlighting the consequences of inadequate server security.",
            Category = "Security",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            UserName = "bySeeder",
            Stars = 1,
            isDisabled = false,
            Picture = "https://wallpapercave.com/wp/wp3797546.jpg",
        };
        dbContext.Articles.Add(article4);

        var article5 = new ArticleModel
        {
            Title = "Cloud Computing: A Developer's Guide to Building Scalable Applications",
            Description = "Introduction: Uncover the fundamentals of cloud computing and its significance in modern application development.\r\nKey Points:\r\nOverview of cloud computing models (IaaS, PaaS, SaaS).\r\nAdvantages of developing applications in the cloud.\r\nPractical tips for designing scalable and resilient cloud-based applications.",
            Category = "Cloud",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            UserName = "bySeeder",
            Stars = 2,
            isDisabled = true,
            Picture = "https://ascenty.com/wp-content/uploads/2016/08/1473150_Ascenty_OtimizacaodeArtigo_05_VantagensservicosCloud_Mai23capablog-1920x1000-c-default.jpg",
        };
        dbContext.Articles.Add(article5);

        dbContext.SaveChanges();
    }

}


//Route Protection 
app.Use(async (context, next) =>
{
    var user = context.User;
    if (context.Request.Path.StartsWithSegments("/AdminPanel") && !user.IsInRole("Administrator"))
    {
        context.Response.StatusCode = 403;
        context.Response.ContentType = "text/html";

        await context.Response.WriteAsync(@"
            <script>
                alert('Access Denied! You do not have permission to access this page.');
                window.location.href = '/';
            </script>
        ");
        return;
    }
    if (context.Request.Path.StartsWithSegments("/Article/Edit"))
    {
        var articleIdString = context.Request.Path.Value.Split("/").Last(); 
        if (int.TryParse(articleIdString, out int articleId))
        {
            var articleRepository = context.RequestServices.GetRequiredService<IArticleRepository>();
            var article = articleRepository.GetArticleById(articleId);

            if (article == null || (!user.Identity.Name.Equals(article.UserName) && !user.IsInRole("Administrator")))
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "text/html";

                await context.Response.WriteAsync(@"
                    <script>
                        alert('Access Denied! You do not have permission to edit this article.');
                        window.location.href = '/';
                    </script>
                ");
                return;
            }
        }
    }
    await next();
});

app.MapControllers();

app.Run();
