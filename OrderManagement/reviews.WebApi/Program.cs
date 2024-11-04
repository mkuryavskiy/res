using Microsoft.EntityFrameworkCore;
using MediatR;
using AutoMapper;
using reviews.Infrastructure.Persistence;
using reviews.Infrastructure.Persistence.Repositories;
using reviews.Application.Interfaces;
using reviews.Application.Services;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using reviews.Application.Orders.Commands.CreateOrder; // Додано для CreateOrder
using reviews.Application.Orders.Commands.UpdateOrder; // Додано для UpdateOrder
using reviews.Application.Orders.Commands.DeleteOrder; // Додано для DeleteOrder
using reviews.Application.Orders.Queries.GetOrders; // Додано для GetOrders
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Налаштування бази даних
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Додавання MediatR
builder.Services.AddMediatR(cfg =>
{
    // Регістрація всіх команд і запитів у проекті
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>(); // Для CreateOrder
    cfg.RegisterServicesFromAssemblyContaining<UpdateOrderCommand>(); // Для UpdateOrder
    cfg.RegisterServicesFromAssemblyContaining<DeleteOrderCommand>(); // Для DeleteOrder
    cfg.RegisterServicesFromAssemblyContaining<GetOrdersQuery>(); // Для GetOrders
});

// Додавання AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Додавання FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommand>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Додавання CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin() // Змініть на конкретні джерела у продакшені
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Додавання контролерів з налаштуваннями JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Додаємо підтримку циклічних посилань
    });

// Додавання Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

// Додавання сервісів застосунку
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Налаштування Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();
app.Run();
