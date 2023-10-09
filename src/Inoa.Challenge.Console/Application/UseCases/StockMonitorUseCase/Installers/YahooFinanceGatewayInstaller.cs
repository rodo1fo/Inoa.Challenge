using System.Net;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Gateways.YahooFinanceGateway;
using Inoa.Challenge.Console.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Installers;

public static class YahooFinanceGatewayInstaller
{
    public static IServiceCollection InstallYahooFinanceGateway(this IServiceCollection collection, IConfiguration config)
    {
        var options = config.GetSection(nameof(YahooFinanceSettings)).Get<YahooFinanceSettings>();

        ArgumentNullException.ThrowIfNull(options);

        collection.AddRefitClient<IYahooFinanceApi>()
        .ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(options.Url);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", options.ApiHost);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", options.ApiKey);
        })
        .AddPolicyHandler(HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(response => response.StatusCode == HttpStatusCode.NoContent)
            .RetryAsync(3, onRetry: (exception, retryCount, context) =>
            {
                // ficou um writeLine para simplificar. O ideal é criar um delegateHandler para conseguir injetar o ILogger
                System.Console.WriteLine($"Retry {retryCount} - {context.PolicyKey}, Exception: {exception?.Exception?.Message} Result: {exception?.Result?.ReasonPhrase}");
            }));

        return collection;
    }
}
