using CSharpFunctionalExtensions;

namespace FunWithFlights.Core.Extensions;

public static class EnumerableExtensions
{
    public static IReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> items) 
        => items switch
        {
            IReadOnlyCollection<T> readOnlyCollection => readOnlyCollection,
            _ => new List<T>(items)
        };

    public static IEnumerable<T> SelectValid<T>(this IEnumerable<Maybe<T>> enumeration)
        => enumeration.Where(x => x.HasValue).Select(x => x.ValueOrDefault());
    
    public static async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(this IEnumerable<T> enumeration, Func<T, Task<IEnumerable<T1>>> func) 
        => (await Task.WhenAll(enumeration.Select(func))).SelectMany(s => s);
    
    public static async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(this IEnumerable<T> enumeration, Func<T, Task<IReadOnlyCollection<T1>>> func) 
        => (await Task.WhenAll(enumeration.Select(func))).SelectMany(s => s);

    public static async Task<IEnumerable<T1>> SelectAsync<T, T1>(this IEnumerable<T> enumeration, Func<T, Task<T1>> func)
        => (await Task.WhenAll(enumeration.Select(func))).Select(s => s);
    
    public static (IEnumerable<T>, IEnumerable<T>) Partition<T>(this IEnumerable<T> enumeration, Func<T, bool> func)
    {
        var groups = enumeration.GroupBy(func).ToDictionary(k => k.Key, v=> v.AsEnumerable());
        return (groups.MaybeGet(true).GetValueOrDefault(Array.Empty<T>()),
                groups.MaybeGet(false).GetValueOrDefault(Array.Empty<T>()));
    }
}