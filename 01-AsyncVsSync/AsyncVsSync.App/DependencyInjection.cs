using Microsoft.Extensions.DependencyInjection;

namespace AsyncVsSync.App;

public static class DependencyInjection
{
    public static ServiceProvider CreateServiceProvider() => new ServiceCollection().AddSerilogLogging()
                                                                                    .AddHttpClient()
                                                                                    .BuildServiceProvider();
}