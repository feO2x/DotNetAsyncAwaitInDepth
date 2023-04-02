using Serilog;
using Serilog.Events;

namespace AsyncVsSync.Backend;

public static class Logging
{
    public static ILogger CreateLogger() =>
        new LoggerConfiguration().MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                                 .WriteTo.Console()
                                 .CreateLogger();
}