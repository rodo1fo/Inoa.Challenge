namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Gateways.YahooFinanceGateway;

public class YahooFinanceResponse
{
    public required QuoteResponse QuoteResponse { get; set; }
}

public class QuoteResponse
{
    public required List<Quote> Result { get; set; }
    public string? Error { get; set; }
}

public class Quote
{
    public decimal regularMarketPrice { get; set; }
}