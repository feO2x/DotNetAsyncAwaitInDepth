using System;
using System.Diagnostics;
using Avalonia;

namespace AsyncDecompiled.AvaloniaApp;

public static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        var appBuilder = BuildAvaloniaApp();
        try
        {
            return appBuilder.StartWithClassicDesktopLifetime(args);
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception.ToString());
            return 1;
        }
    }
    
    private static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
                  .UsePlatformDetect()
                  .LogToTrace();
}