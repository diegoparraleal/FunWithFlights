namespace FunWithFlights.Core.Queries;

public interface IQueryAll<TOut>
{
    Task<IReadOnlyCollection<TOut>> GetAllAsync();
}