using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Gateways.YahooFinanceGateway;

namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Models;

public class Asset
{
    required public decimal MarketPrice { get; set; }

    public static Asset FromYahooFinance(YahooFinanceResponse response) => new Asset
    {
        MarketPrice = response.QuoteResponse.Result.First().regularMarketPrice
    };
}