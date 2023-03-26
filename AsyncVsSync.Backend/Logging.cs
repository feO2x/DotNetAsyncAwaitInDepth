using Serilog;

namespace AsyncVsSync.Backend;

public static class Logging
{
    public static ILogger CreateLogger() =>
        new LoggerConfiguration().MinimumLevel.Warning()
                                 .WriteTo.Console()
                                 .CreateLogger();
}