using System;
using System.Threading.Tasks;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Installers;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Models;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Abstractions;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Installers;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Models;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Output;
using Inoa.Challenge.Console.Infrastructure.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using StockMonitorUseCaseInstaller;

namespace Inoa.Challenge.Console;

public class Program
{
    static ServiceProvider? services;
    static IConfiguration? configuration;

    static async Task Main(string[] args)
    {
        InstallDependencies();

        var log = services!.GetRequiredService<ILogger<Program>>();
        var stockMonitor = services!.GetRequiredService<IStockMonitorUseCase>();
        var notification = services!.GetRequiredService<INotificationUseCase>();
        var interval = configuration!.GetValue<int>("interval");

        System.Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] Program started");
        log.LogInformation("program started");

        try
        {
            var stockModel = StockMonitorModel.FromArgs(args);

            while (true)
            {
                var monitorOutput = await stockMonitor.ExecuteAsync(stockModel);

                if (monitorOutput.ShouldNotify)
                {
                    var notificationModel = CreateNotificationModel(stockModel, monitorOutput);

                    await notification.SendNotificationAsync(notificationModel);
                }

                await Task.Delay(interval);
            }
        }
        catch (ArgumentException)
        {
            System.Console.WriteLine("parâmetros inválidos, ex.: dotnet run stock.dll -a PETR4 -s 22.67 -b 22.59");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unexpected error. Application stopped");
        }
    }

    private static NotificationModel CreateNotificationModel(StockMonitorModel model, StockMonitorOutput output) =>
        new NotificationModel
        {
            Operation = output.Operation,
            NotificationChannel = model.NotificationChannel,
            AssetName = model.AssetName,
            StartPrice = output.Operation == Application.Models.OperationType.Buy ? model.BuyingReferencePrice : model.SellingReferencePrice,
            EndPrice = output.MarketPrice
        };

    private static void InstallDependencies()
    {
        var serviceCollection = new ServiceCollection();

        // Configurar o provedor de configuração
        configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        configuration
            .AddYahooServiceSettings()
            .AddEmailSettings(serviceCollection);

        // Configuração da injeção de dependência
        serviceCollection
            .AddSerilog()
            .InstallStockMonitorUseCase()
            .InstallNotificationUseCase()
            .InstallYahooFinanceGateway(configuration)
            .InstallGenericSmtpGateway()
            .InstallRabbitMqGateway();

        services = serviceCollection.BuildServiceProvider();
    }
}