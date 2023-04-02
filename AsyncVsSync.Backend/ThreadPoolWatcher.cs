using System;
using System.Threading;

namespace AsyncVsSync.Backend;

public sealed class ThreadPoolWatcher
{
    public readonly int MaximumWorkerThreads;
    private int _maximumUsedWorkerThreads;

    public ThreadPoolWatcher() => ThreadPool.GetMaxThreads(out MaximumWorkerThreads, out _);

    public int MaximumUsedWorkerThreads => Volatile.Read(ref _maximumUsedWorkerThreads);

    public void UpdateUsedThreads()
    {
        var currentlyUsedWorkerThreads = GetNumberOfUsedWorkerThreads();
        InterlockedMaximum(ref _maximumUsedWorkerThreads, currentlyUsedWorkerThreads);
    }

    private int GetNumberOfUsedWorkerThreads()
    {
        ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out _);
        return MaximumWorkerThreads - availableWorkerThreads;
    }

    private static void InterlockedMaximum(ref int target, int value)
    {
        int temporaryValue;
        var readValueOfTarget = target;
        do
        {
            temporaryValue = readValueOfTarget;
            readValueOfTarget =
                Interlocked.CompareExchange(ref target, Math.Max(temporaryValue, value), temporaryValue);
        } while (temporaryValue != readValueOfTarget);
    }

    public void Reset() => Volatile.Write(ref _maximumUsedWorkerThreads, 0);
}