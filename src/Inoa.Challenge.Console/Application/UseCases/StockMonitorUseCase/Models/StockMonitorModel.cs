using CommandLine;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Models;

namespace Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Models;

public class StockMonitorModel
{
    [Option('a', "asset", Required = true, HelpText = "Ativo a ser monitorado")]
    public string AssetName { get; set; }

    [Option('s', "sell", Required = true, HelpText = "Preço de referência para venda")]
    public decimal SellingReferencePrice { get; set; }

    [Option('b', "buy", Required = true, HelpText = "Preço de referência para compra")]
    public decimal BuyingReferencePrice { get; set; }

    [Option('c', "channel", Required = false, HelpText = "\"Email\" para enviar diretamente por email ou \"Queue\" para delegar para outro serviço.")]
    public NotificationChannel NotificationChannel { get; set; } = NotificationChannel.Email;

    public static StockMonitorModel FromArgs(string[] args)
    {
        var parser = new Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<StockMonitorModel>(args);

        if (result?.Value is null)
            throw new ArgumentException();

        return result.Value;
    }
}