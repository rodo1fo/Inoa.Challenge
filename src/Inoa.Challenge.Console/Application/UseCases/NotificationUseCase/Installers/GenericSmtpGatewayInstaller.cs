using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Gateways.GenericSmtpGateway;
using Microsoft.Extensions.DependencyInjection;

namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Installers;

public static class GenericSmtpGatewayInstaller
{
    public static IServiceCollection InstallGenericSmtpGateway(this IServiceCollection collection)
    {
        collection.AddScoped<IGenericSmtpApi, GenericSmtpApi>();

        return collection;
    }
}
