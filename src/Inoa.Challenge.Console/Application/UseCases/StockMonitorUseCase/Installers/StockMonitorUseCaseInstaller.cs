using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace StockMonitorUseCaseInstaller;

public static class StockMonitorUseCaseInstaller
{
    public static IServiceCollection InstallStockMonitorUseCase(this IServiceCollection collection)
    {
        collection
        .AddScoped<IStockMonitorUseCase, StockMonitorUseCase>();

        return collection;
    }
}