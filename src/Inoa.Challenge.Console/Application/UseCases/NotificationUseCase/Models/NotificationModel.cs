using Inoa.Challenge.Console.Application.Models;

namespace Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Models;

public record NotificationModel
{
    required public OperationType Operation { get; set; }
    required public NotificationChannel NotificationChannel { get; set; }
    required public string AssetName { get; set; }
    required public decimal StartPrice { get; set; }
    required public decimal EndPrice { get; set; }
}