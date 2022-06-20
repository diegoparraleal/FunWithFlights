using CSharpFunctionalExtensions;

namespace FunWithFlights.Core.Extensions;

public static class MaybeExtensions
{
    public static bool TryGet<T>(this Maybe<T> maybe, out T obj)
    {
        obj = default!;
        if (maybe.HasValue) obj = maybe.ValueOrDefault();
        return maybe.HasValue;
    }
    
    public static Maybe<T> AsMaybe<T>(this T obj) => obj != null ? Maybe.From(obj): Maybe.None;
    
    public static Maybe<T> Cast<T>(object? obj) where T : class => obj is T tObj? Maybe.From(tObj): Maybe<T>.None;
    
    public static Maybe<T> MaybeCast<T>(this object obj) where T : class => Cast<T>(obj);
    
    public static Maybe<TOut> Then<T, TOut>(this Maybe<T> maybe, Func<T, TOut> predicate)
        => maybe.TryGet(out var obj) ? predicate(obj): Maybe<TOut>.None;
    
    public static T ValueOrDefault<T>(this Maybe<T> maybe, Func<T> predicate) => maybe.GetValueOrDefault(predicate());
    
    public static T ValueOrDefault<T>(this Maybe<T> maybe, T defaultValue) => maybe.GetValueOrDefault(defaultValue);
    public static T ValueOrDefault<T>(this Maybe<T> maybe) => maybe.GetValueOrDefault();

    public static Maybe<int> MaybeParseAsInt(this string text) => int.TryParse(text, out int parsed) ? parsed : Maybe<int>.None;

    public static Maybe<T> Execute<T>(Func<T> fn)
    {
        try
        {
            return fn();
        }
        catch
        {
            return Maybe<T>.None;
        }
    }
    
    public static async Task<Maybe<T>> Execute<T>(Func<Task<T>> fn)
    {
        try
        {
            return await fn();
        }
        catch
        {
            return Maybe<T>.None;
        }
    }
}