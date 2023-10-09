using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Models;
using Microsoft.Extensions.Logging;

namespace Inoa.Challenge.Console.Application.UseCases.NotificationUseCase;

public class NotificationUseCase : INotificationUseCase
{
    private readonly IGenericSmtpApi _smtp;
    private readonly IRabbitMqApi _rabbitMq;
    private readonly ILogger<NotificationUseCase> _logger;

    public NotificationUseCase(ILogger<NotificationUseCase> logger, IGenericSmtpApi smtp, IRabbitMqApi rabbitMq)
    {
        _logger = logger;
        _rabbitMq = rabbitMq;
        _smtp = smtp;
    }

    public async Task SendNotificationAsync(NotificationModel model)
    {
        _logger.LogInformation("{name} started. Model:{model}", nameof(NotificationUseCase), model.ToString());

        if (model.NotificationChannel == NotificationChannel.Email)
            _smtp.SendEmail(model);
        else
            await _rabbitMq.SendMessageAsync(model);

        _logger.LogInformation("{name} completed. Sent by {operation}", nameof(NotificationUseCase), model.Operation);
    }
}