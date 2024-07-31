using CryptoWorld.News.Core.Services;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CryptoWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.Services.News;
using CryptоWorld.News.Core.ViewModels.HomePage;
using Serilog;
using CryptoWorld.News.Core.ExceptionHandler;
using CryptoWorld.News.Core.Services.News;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services
    .AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                });
builder.Services.AddCors();
builder.Services.AddScoped<IAccountService,AccountService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<UrlForNews>();
builder.Services.Configure<UrlForNews>(builder.Configuration.GetSection("MoneyBgUrl"));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddTransient<IAlertService, AlertService>();
builder.Services.AddScoped<RssFeedService>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/CryptoNewsLogsFromSerilog-.txt",rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

//Apply migrations after project run
using (var serviceScope = app.Services.CreateScope())
{
    if (app.Environment.IsDevelopment())
    {
        try
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        }
    }
}
app.UseDefaultFiles();
app.UseStaticFiles();
//Middleware will work fine with this code
app.UseExceptionHandler(_ => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();