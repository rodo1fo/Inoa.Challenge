using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Models;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Output;

namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Abstractions;

interface IStockMonitorUseCase
{
    Task<StockMonitorOutput> ExecuteAsync(StockMonitorModel model);
}