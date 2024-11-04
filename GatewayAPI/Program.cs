using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

var builder = WebApplication.CreateBuilder(args);

// Додаємо базові сервіси
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Підключаємо файл конфігурації Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot().AddSingletonDefinedAggregator<CustomAggregator>();

var app = builder.Build();

// Додаємо Swagger тільки в режимі розробки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Використовуємо Ocelot як middleware
await app.UseOcelot();

// Маршрутизація контролерів
app.MapControllers();

app.Run();
