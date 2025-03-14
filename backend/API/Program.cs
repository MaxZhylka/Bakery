using backend.Core.Interfaces;
using backend.Core.Services;
using backend.Infrastructure.Database;
using backend.Infrastructure.Interfaces;
using backend.Infrastructure.Repositories;
using backend.Core.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend.Core.Middlewares;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IDBConnectionFactory, DBConnectionFactory>();

builder.Services.AddControllers();


builder.Services.AddAutoMapper(typeof(MappingProfile));

using var _customFontStream = new MemoryStream(File.ReadAllBytes("./API/Fonts/OpenSans_Condensed-Light.ttf"));
FontManager.RegisterFont(_customFontStream);
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<ILoggerRepository, LoggerRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentityServer();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddCors(options =>
{
    var clientUrl = DotNetEnv.Env.GetString("CLIENT_URL") ?? "http://localhost:4200";
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins(clientUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your-app",
            ValidAudience = "your-app",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DotNetEnv.Env.GetString("SECRET_KEY")))
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseMiddleware<LoggerMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
