using CSharpFunctionalExtensions;

namespace FunWithFlights.Core.Entities;

public interface IReadOnlyRepository<TKey, TEntity> where TEntity: IEntity<TKey> where TKey : notnull
{
    public Task<IReadOnlyCollection<TEntity>> GetAllAsync();
    public Task<Maybe<TEntity>> GetByKeyAsync(TKey key);
    public Task<int> CountAsync();
}