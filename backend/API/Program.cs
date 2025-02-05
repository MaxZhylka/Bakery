using backend.Core.Interfaces;
using backend.Core.Services;
using backend.Infrastructure.Database;
using backend.Infrastructure.Interfaces;
using backend.Infrastructure.Repositories;
using backend.Core.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IDBConnectionFactory, DBConnectionFactory>();

builder.Services.AddControllers();


builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();


app.MapControllers();

app.Run();
