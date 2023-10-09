using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Gateways.RabbitMqGateway;
using Microsoft.Extensions.DependencyInjection;

namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Installers;

public static class RabbitMqGatewayInstaller
{
    public static IServiceCollection InstallRabbitMqGateway(this IServiceCollection collection)
    {
        collection.AddScoped<IRabbitMqApi, RabbitMqApi>();

        return collection;
    }
}
