namespace FunWithFlights.Core.Commands;

public interface ICommandParameters{ } 
public interface ICommandResult{ } 

public interface ICommand<in TIn, TOut> 
    where TIn: ICommandParameters
    where TOut: ICommandResult
{
    Task<TOut> ExecuteAsync(TIn parameters);
}