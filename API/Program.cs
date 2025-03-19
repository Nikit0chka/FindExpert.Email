using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Kafka.SendAuthorizationCode;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var loggerFactory = LoggerFactory.Create(static loggingBuilder =>
{
    loggingBuilder.AddSerilog(); // Интеграция Serilog
});

var logger = loggerFactory.CreateLogger("Program");

builder.Services.AddInfrastructureServices(builder.Configuration, logger);
builder.Services.AddApplicationServices(logger);
builder.Services.AddKafkaConsumer<SendAuthorizationCodeMessage, SendAuthorizationCodeHandler>(builder.Configuration.GetSection("Kafka:SendAuthorizationCode"), logger);

var app = builder.Build();

app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(static c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.Run();