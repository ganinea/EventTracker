using System.Runtime.InteropServices;

namespace EventTracker.Common.Extensions;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
    {
        if (self is List<T> list)
        {
            foreach (var item in CollectionsMarshal.AsSpan(list))
            {
                action(item);
            }

            return;
        }

        foreach (var item in self)
        {
            action(item);
        }
    }

    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        return !source.Any();
    }

    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
    {
        return enumerable.Where(e => e != null).Select(e => e!);
    }
}