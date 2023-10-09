using Refit;

namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Gateways.YahooFinanceGateway;

public interface IYahooFinanceApi
{
    [Get("/market/v2/get-quotes?region=US&symbols={symbol}.SA")]
    Task<YahooFinanceResponse> GetStockAsync(string symbol);
}