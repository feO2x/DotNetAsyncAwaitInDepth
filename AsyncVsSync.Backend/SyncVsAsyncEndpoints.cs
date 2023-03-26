using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Light.Validation;
using Light.Validation.Checks;
using Light.Validation.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AsyncVsSync.Backend;

public static class SyncVsAsyncEndpoints
{
    public static WebApplication MapSyncVsAsyncEndpoints(this WebApplication app)
    {
        app.MapGet("/sync", WaitSync);
        app.MapGet("/async", DelayAsync);
        app.MapGet("/results", GetThreadPoolResults);
        return app;
    }

    private static IResult WaitSync(int waitIntervalInMilliseconds,
                                    ValidationContext validationContext,
                                    ThreadPoolWatcher threadPoolWatcher)
    {
        if (validationContext.CheckForErrors(waitIntervalInMilliseconds, out var errors))
            return Results.BadRequest(errors);

        Thread.Sleep(waitIntervalInMilliseconds);
        threadPoolWatcher.UpdateUsedThreads();
        return Results.Ok();
    }

    private static async Task<IResult> DelayAsync(int waitIntervalInMilliseconds,
                                                  ValidationContext validationContext,
                                                  ThreadPoolWatcher threadPoolWatcher)
    {
        if (validationContext.CheckForErrors(waitIntervalInMilliseconds, out var errors))
            return Results.BadRequest(errors);

        await Task.Delay(waitIntervalInMilliseconds);
        threadPoolWatcher.UpdateUsedThreads();
        return Results.Ok();
    }

    private static IResult GetThreadPoolResults(ThreadPoolWatcher threadPoolWatcher)
    {
        var results = new { threadPoolWatcher.UsedWorkerThreads, threadPoolWatcher.MaximumWorkerThreads };
        return Results.Ok(results);
    }

    private static bool CheckForErrors(this ValidationContext context,
                                       int waitIntervalInMilliseconds,
                                       [NotNullWhen(true)] out object? errors)
    {
        context.Check(waitIntervalInMilliseconds).IsIn(Range.FromInclusive(15).ToInclusive(60_000));
        return context.TryGetErrors(out errors);
    }
}