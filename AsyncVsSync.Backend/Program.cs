using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace AsyncVsSync.Backend;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = Logging.CreateLogger();
    
        try
        {
            var app = WebApplication.CreateBuilder(args)
                                    .ConfigureServices()
                                    .Build()
                                    .MapSyncVsAsyncEndpoints();

            // Do not run performance tests in Debug mode. Also, make sure that you enabled
            // best performance in your OS settings and that your laptop is plugged in.
#if DEBUG
            Log.Warning("Do not run performance tests in Debug mode - please switch to Release mode");
#endif

            await app.RunAsync();
            return 0;
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Could not run AsyncVsSync backend");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }

    }
}