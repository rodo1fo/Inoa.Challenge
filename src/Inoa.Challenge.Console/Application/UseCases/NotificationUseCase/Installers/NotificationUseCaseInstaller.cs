using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Installers;

public static class NotificationUseCaseInstaller
{
    public static IServiceCollection InstallNotificationUseCase(this IServiceCollection collection)
    {
        collection.AddScoped<INotificationUseCase, NotificationUseCase>();

        return collection;
    }
}