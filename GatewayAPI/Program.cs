using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

var builder = WebApplication.CreateBuilder(args);

// ������ ����� ������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// ϳ�������� ���� ������������ Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot().AddSingletonDefinedAggregator<CustomAggregator>();

var app = builder.Build();

// ������ Swagger ����� � ����� ��������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ������������� Ocelot �� middleware
await app.UseOcelot();

// ������������� ����������
app.MapControllers();

app.Run();
