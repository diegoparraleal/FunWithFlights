namespace FunWithFlights.Core.Queries;

public interface IQueryByName<TOut>
{
    Task<TOut> GetByNameAsync(string name);
}