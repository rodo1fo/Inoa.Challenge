using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Inoa.Challenge.Console.Infrastructure.Installers
{
    public static class LoggerInstaller
    {
        public static IServiceCollection AddSerilog(this IServiceCollection collection)
        {
            collection.AddLogging(builder =>
            {
                builder.ClearProviders();

                string executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File(Path.Combine(executingPath, "logs", "log.txt"), rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                builder.AddSerilog(logger);
            });

            return collection;
        }
    }
}