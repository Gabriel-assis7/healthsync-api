namespace HealthSync.BuildingBlocks.Abstraction.Commands
{
    public interface ICommand<TResult>
    {
    }

    public interface IQuery<TResult> : ICommand<TResult>
    {
    }
}
