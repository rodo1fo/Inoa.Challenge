using System.Threading.Tasks;
using Inoa.Challenge.Console.Application.Models;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Inoa.Challenge.Console.UnitTest
{
    public class NotificationUseCaseTest
    {
        Mock<ILogger<NotificationUseCase>> loggerMock;
        Mock<IGenericSmtpApi> smtpApiMock;
        Mock<IRabbitMqApi> rabbitMqApiMock;
        NotificationUseCase notificationUseCase;
        NotificationModel model;

        public NotificationUseCaseTest()
        {
            loggerMock = new Mock<ILogger<NotificationUseCase>>();
            smtpApiMock = new Mock<IGenericSmtpApi>();
            rabbitMqApiMock = new Mock<IRabbitMqApi>();
            notificationUseCase = new NotificationUseCase(loggerMock.Object, smtpApiMock.Object, rabbitMqApiMock.Object);

            model = new NotificationModel
            {
                AssetName = "PETR4",
                StartPrice = 5,
                EndPrice = 10,
                Operation = OperationType.Buy,
                NotificationChannel = NotificationChannel.Email
            };
        }

        [Fact]
        public async Task ExecuteAsync_ShouldNotifyByEmail_When_NotificationChannelIsEmail()
        {
            // Arrange
            var m = model with { NotificationChannel = NotificationChannel.Email };

            // Act
            await notificationUseCase.SendNotificationAsync(m);

            // Assert
            smtpApiMock.Verify(x => x.SendEmail(It.IsAny<NotificationModel>()), Times.Once);
            rabbitMqApiMock.Verify(x => x.SendMessageAsync(It.IsAny<NotificationModel>()), Times.Never);
        }


        [Fact]
        public async Task ExecuteAsync_ShouldNotifyByRabbitMq_When_NotificationChannelIsQueue()
        {
            // Arrange
            var m = model with { NotificationChannel = NotificationChannel.Queue };

            // Act
            await notificationUseCase.SendNotificationAsync(m);

            // Assert
            smtpApiMock.Verify(x => x.SendEmail(It.IsAny<NotificationModel>()), Times.Never);
            rabbitMqApiMock.Verify(x => x.SendMessageAsync(It.IsAny<NotificationModel>()), Times.Once);
        }
    }
}