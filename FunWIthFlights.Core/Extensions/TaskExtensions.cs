namespace FunWithFlights.Core.Extensions;

public static class TaskExtensions
{
    public static Task<T> AsTask<T>(this T obj) => Task.FromResult(obj);
    public static Task<IReadOnlyCollection<T>> AsReadOnlyTask<T>(this IEnumerable<T> obj) => obj.AsReadOnly().AsTask();
}