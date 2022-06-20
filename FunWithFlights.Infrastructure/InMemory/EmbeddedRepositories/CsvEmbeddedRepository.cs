using System.Globalization;
using CSharpFunctionalExtensions;
using CsvHelper;
using CsvHelper.Configuration;
using FunWithFlights.Core.Entities;
using FunWithFlights.Core.Extensions;
using MaybeExtensions = FunWithFlights.Core.Extensions.MaybeExtensions;

namespace FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

public abstract class CsvEmbeddedRepository<TKey, T>: IReadOnlyRepository<TKey, T> where T: IEntity<TKey> where TKey : notnull
{
    private readonly Stream _stream;
    private readonly SemaphoreSlim _semaphoreSlim = new (1, 1);
    private IReadOnlyDictionary<TKey, T>? _store;
    private readonly CsvConfiguration _configuration;

    protected CsvEmbeddedRepository(string resource)
    {
        if(resource == null) throw new ArgumentException($"CSV embedded resource must be provided");
        
        _stream = AppDomain.CurrentDomain
            .GetAssemblies()
            .TryFirst(x => !x.IsDynamic && x.GetManifestResourceNames().Contains(resource))
            .GetValueOrThrow($"CSV resource {resource} cannot be found in current assemblies")
            .GetManifestResourceStream(resource)!;

        if (_stream == null) throw new ArgumentException($"Stream from {resource} is null");
        _configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            NewLine = Environment.NewLine,
            DetectDelimiter = true,
            HasHeaderRecord = true,
            MissingFieldFound = null,
            TrimOptions = TrimOptions.Trim
        };
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync() => (await GetStore()).Values.AsReadOnly();

    public async Task<Maybe<T>> GetByKeyAsync(TKey key)
    {
        var store = await GetStore();
        return (store as IDictionary<TKey, T>)!.MaybeGet(key);
    }

    public async Task<int> CountAsync() => (await GetStore()).Count;
    
    private async Task<IReadOnlyDictionary<TKey, T>> LoadFile()
    {
        var result = new Dictionary<TKey, T>();
        using var reader = new StreamReader(_stream);
        using var csv = new CsvReader(reader, _configuration);
        
        await csv.ReadAsync();
        csv.ReadHeader();
        while (await csv.ReadAsync())
        {
            var record = csv.HeaderRecord.ToDictionary(k => k, v => csv.GetField(v));
            var maybeMapped = MaybeExtensions.Execute(() => MapRecord(record)); 
            if (maybeMapped.TryGet(out var mapped)) result.TryAdd(mapped.Key, mapped);
        }

        return result;
    }

    protected abstract T MapRecord(Dictionary<string, string> record);

    private async Task<IReadOnlyDictionary<TKey, T>> GetStore()
    {
        if (_store != null) return _store;

        await _semaphoreSlim.WaitAsync();
        try
        {
            if (_store != null) return _store;
            _store = await LoadFile();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
        return _store;
    }
}