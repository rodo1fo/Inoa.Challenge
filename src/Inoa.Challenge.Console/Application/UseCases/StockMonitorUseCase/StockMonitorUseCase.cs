using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Abstractions;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Gateways.YahooFinanceGateway;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Models;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Output;
using Microsoft.Extensions.Logging;

namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase;

public class StockMonitorUseCase : IStockMonitorUseCase
{
    private readonly ILogger<StockMonitorUseCase> _logger;
    private readonly IYahooFinanceApi _yahooApi;

    public StockMonitorUseCase(ILogger<StockMonitorUseCase> logger, IYahooFinanceApi yahooApi)
    {
        _logger = logger;
        _yahooApi = yahooApi;
    }

    public async Task<StockMonitorOutput> ExecuteAsync(StockMonitorModel model)
    {
        _logger.LogInformation("sending request. Request: {stockModel}", model);

        var apiResponse = await _yahooApi.GetStockAsync(model.AssetName);

        _logger.LogInformation("request sent. Response: {apiResponse}", apiResponse);

        var asset = Asset.FromYahooFinance(apiResponse);

        if (asset.MarketPrice >= model.SellingReferencePrice || asset.MarketPrice <= model.BuyingReferencePrice)
        {
            var output = StockMonitorOutput.CreateFrom(asset, model, shouldNotify: true);

            Print($"Asset price R${asset.MarketPrice}. Should {output.Operation.ToString()}");

            return output;
        }

        Print($"Asset price R${asset.MarketPrice}. Waiting...");

        return StockMonitorOutput.CreateFrom(asset, model, shouldNotify: false);
    }

    private void Print(string s) => System.Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss")}] {s}");
}