namespace FunWithFlights.Core.Aggregators;

public interface IAggregatorParameters{ } 

public interface IAggregator<TOut>
{
    Task<TOut> ExecuteAsync();
}

public interface IAggregator<in TIn, TOut> where TIn: IAggregatorParameters
{
    Task<TOut> ExecuteAsync(TIn parameters);
}