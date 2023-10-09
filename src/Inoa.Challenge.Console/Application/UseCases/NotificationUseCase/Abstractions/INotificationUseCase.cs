using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Models;

namespace Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;

public interface INotificationUseCase
{
    Task SendNotificationAsync(NotificationModel model);
}