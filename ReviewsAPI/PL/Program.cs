using System.Data;
using System.Data.SqlClient;
using DAL.Data;
using DAL.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add API explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Reviews API", Version = "v1" });
});

// Register database connection
builder.Services.AddTransient<IDbConnection>(_ =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register repositories
builder.Services.AddTransient<ReviewRepository>();
builder.Services.AddTransient<UserRepository>();

// Register in-memory caching with size limit
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024; // встановити обмеження розміру кешу
});

// Register InMemoryCacheService
builder.Services.AddScoped<InMemoryCacheService>();

// Register Redis distributed caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "RedisCache_"; // префікс ключів для Redis
});

// Register RedisCacheService
builder.Services.AddScoped<RedisCacheService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reviews API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger at the root URL
    });
}

app.MapControllers();
app.Run();
