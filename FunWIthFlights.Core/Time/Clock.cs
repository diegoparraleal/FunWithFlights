namespace FunWithFlights.Core.Time;

public interface IClock
{
    DateTime Now { get; }
}

public class Clock: IClock
{
    public DateTime Now => DateTime.Now;
}