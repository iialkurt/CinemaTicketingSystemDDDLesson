namespace CinemaTicketingSystem.Application.DependencyInjections;

public interface ITransientDependency;
public interface IScopedDependency;
public interface ISingletonDependency;



public interface IExampleService
{
    void DoWork();
}

public class ExampleService : IExampleService, ITransientDependency
{
    public void DoWork()
    {
        Console.WriteLine("Doing some work...");
    }
}