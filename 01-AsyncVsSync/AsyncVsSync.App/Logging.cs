using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace AsyncVsSync.App;

public static class Logging
{
    public static LoggingLevelSwitch LoggingLevelSwitch { get; } = new (LogEventLevel.Warning);

    public static ILogger CreateLogger() => new LoggerConfiguration().WriteTo.Console()
                                                                     .MinimumLevel.ControlledBy(LoggingLevelSwitch)
                                                                     .CreateLogger();

    public static IServiceCollection AddSerilogLogging(this IServiceCollection services) => services.AddLogging(builder => builder.AddSerilog())
                                                                                                    .AddSingleton(LoggingLevelSwitch);
}