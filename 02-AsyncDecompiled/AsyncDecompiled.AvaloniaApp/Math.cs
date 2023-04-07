using System.Linq;
using System.Threading.Tasks;

namespace AsyncDecompiled.AvaloniaApp;

public static class Math
{
    public static long CalculateLowestCommonMultiple(int from, int to)
    {
        var denominators = Enumerable.Range(from, to - 1).ToList();
        for (var i = (long) to; i < long.MaxValue; i++)
        {
            if (denominators.All(denominator => i % denominator == 0))
                return i;
        }

        return -1L;
    }

    public static Task<long> CalculateLowestCommonMultipleAsync(int from, int to)
    {
        return Task.Run(() => CalculateLowestCommonMultiple(from, to));
    }
}