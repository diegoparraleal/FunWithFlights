using CSharpFunctionalExtensions;

namespace FunWithFlights.Core.Extensions;

public static class DictionaryExtensions
{
    public static Maybe<T> MaybeGet<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key)
    {
        dictionary.TryGetValue(key, out var result);
        return Maybe<T>.From(result!);
    }
}