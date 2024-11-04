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
using reviews.Application.Orders.Commands.CreateOrder; // ������ ��� CreateOrder
using reviews.Application.Orders.Commands.UpdateOrder; // ������ ��� UpdateOrder
using reviews.Application.Orders.Commands.DeleteOrder; // ������ ��� DeleteOrder
using reviews.Application.Orders.Queries.GetOrders; // ������ ��� GetOrders
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ������������ ���� �����
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� MediatR
builder.Services.AddMediatR(cfg =>
{
    // ���������� ��� ������ � ������ � ������
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>(); // ��� CreateOrder
    cfg.RegisterServicesFromAssemblyContaining<UpdateOrderCommand>(); // ��� UpdateOrder
    cfg.RegisterServicesFromAssemblyContaining<DeleteOrderCommand>(); // ��� DeleteOrder
    cfg.RegisterServicesFromAssemblyContaining<GetOrdersQuery>(); // ��� GetOrders
});

// ��������� AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ��������� FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommand>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin() // ����� �� �������� ������� � ���������
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// ��������� ���������� � �������������� JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // ������ �������� �������� ��������
    });

// ��������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

// ��������� ������ ����������
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// ������������ Middleware
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
