using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Models;

namespace Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;

public interface IRabbitMqApi
{
    Task SendMessageAsync(NotificationModel model);
}