using Application.Contracts;
using Infrastructure.Kafka;
using Infrastructure.Kafka.SendAuthorizationCode;
using Infrastructure.Services;
using Infrastructure.Services.SendEmail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

/// <summary>
/// Service collection extension logic
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add infrastructure services logic
    /// </summary>
    /// <param name="services">Services collection</param>
    /// <param name="configuration">Config</param>
    /// <param name="logger">Logger</param>
    /// <returns></returns>
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        logger.LogInformation("Adding infrastructure services...");

        services.Configure<SendEmailServiceOptions>(configuration.GetSection("SendEmailServiceOptions"));
        services.AddScoped<ISendEmailService, SendEmailService>();
        services.AddScoped<IEmailMessageTemplateService, EmailMessageTemplateService>();

        logger.LogInformation("Infrastructure services added");
    }

    /// <summary>
    /// Add kafka consumer
    /// </summary>
    /// <param name="serviceCollection">Services collection</param>
    /// <param name="configurationSection">Config</param>
    /// <param name="logger">Logger</param>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <typeparam name="THandler">Type of handler</typeparam>
    public static void AddKafkaConsumer<TMessage, THandler>(this IServiceCollection serviceCollection, IConfigurationSection configurationSection, ILogger logger) where THandler : class, IMessageHandler<TMessage>
    {
        logger.LogInformation("Adding kafka consumer for message type {MessageType}", typeof(TMessage).Name);

        serviceCollection.Configure<ConsumerOptions>(configurationSection);
        serviceCollection.AddHostedService<Consumer<TMessage>>();
        serviceCollection.AddSingleton<IMessageHandler<TMessage>, THandler>();
        logger.LogInformation("Added consumer for message type {MessageType}", typeof(TMessage).Name);
    }
}