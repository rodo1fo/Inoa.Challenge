using Inoa.Challenge.Console.Application.Models;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Models;

namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Output;

public class StockMonitorOutput
{
    required public bool ShouldNotify { get; set; }
    required public OperationType Operation { get; set; }
    required public decimal SellingPrice { get; set; }
    required public decimal BuyingPrice { get; set; }
    required public decimal MarketPrice { get; set; }

    internal static StockMonitorOutput CreateFrom(Asset asset, StockMonitorModel stockModel, bool shouldNotify) =>
    new StockMonitorOutput
    {
        ShouldNotify = shouldNotify,
        Operation = asset.MarketPrice >= stockModel.SellingReferencePrice ? OperationType.Sell : OperationType.Buy,
        SellingPrice = stockModel.SellingReferencePrice,
        BuyingPrice = stockModel.BuyingReferencePrice,
        MarketPrice = asset.MarketPrice
    };
}