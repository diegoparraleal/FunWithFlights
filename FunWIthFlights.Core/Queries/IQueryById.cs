namespace FunWithFlights.Core.Queries;

public interface IQueryByKey<TKey, TOut>
{
    Task<TOut> GetByKeyAsync(TKey key);
}