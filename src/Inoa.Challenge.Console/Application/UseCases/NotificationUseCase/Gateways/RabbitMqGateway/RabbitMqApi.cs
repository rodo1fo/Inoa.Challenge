using System.Text;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Models;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Output;
using RabbitMQ.Client;

namespace Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Gateways.RabbitMqGateway;

public class RabbitMqApi : IRabbitMqApi
{
    public Task SendMessageAsync(NotificationModel model)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "stock",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(model.ToString());

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "stock",
                             basicProperties: null,
                             body: body);

        return Task.FromResult(new NotificationOutput());
    }
}