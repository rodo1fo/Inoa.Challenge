using Inoa.Challenge.Console.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inoa.Challenge.Console.Infrastructure.Installers;

public static class ConfigurationInstaller
{
    public static IConfiguration AddAppSettings(this IConfiguration config)
    {
        var options = new AppSettings();
        config.GetSection(nameof(AppSettings)).Bind(options);

        return config;
    }

    public static IConfiguration AddYahooServiceSettings(this IConfiguration config)
    {
        var options = new YahooFinanceSettings();
        config.GetSection(nameof(YahooFinanceSettings)).Bind(options);

        return config;
    }

    public static IConfiguration AddEmailSettings(this IConfiguration config, IServiceCollection collection)
    {
        var options = new EmailSettings();
        config.GetSection(nameof(EmailSettings)).Bind(options);

        collection.Configure<EmailSettings>(config.GetSection(nameof(EmailSettings)));

        return config;
    }
}