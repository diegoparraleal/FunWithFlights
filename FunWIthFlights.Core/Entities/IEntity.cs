namespace FunWithFlights.Core.Entities;

public interface IEntity<TKey>
{
    public TKey Key { get; }
}