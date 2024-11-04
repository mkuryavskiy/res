using CategoriesAPI.Data;
using CategoriesAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register the DbContext with the connection string from configuration
builder.Services.AddDbContext<CategoriesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services and repositories
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

// Configure Swagger with custom settings
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Categories API",
        Version = "v1",
        Description = "API for managing categories and items",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Categories API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger at the root URL
    });
}
else
{
    // Use exception handler for production
    app.UseExceptionHandler("/error");
    app.UseHsts(); // Use HSTS in production
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
