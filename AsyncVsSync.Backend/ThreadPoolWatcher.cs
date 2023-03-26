using System;
using System.Threading;

namespace AsyncVsSync.Backend;

public sealed class ThreadPoolWatcher
{
    public readonly int MaximumWorkerThreads;
    private int _usedWorkerThreads;

    public ThreadPoolWatcher() => ThreadPool.GetMaxThreads(out MaximumWorkerThreads, out _);

    public int UsedWorkerThreads => Volatile.Read(ref _usedWorkerThreads);

    public void UpdateUsedThreads()
    {
        ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out _);
        InterlockedMaximum(ref _usedWorkerThreads, MaximumWorkerThreads - availableWorkerThreads);
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
}