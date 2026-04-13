namespace HealthSync.BuildingBlocks.Abstraction.Contexts
{
    public interface IHeaders : IEnumerable<KeyValuePair<string, object>>
    {
        IEnumerable<KeyValuePair<string, object>> GetAll();
    }
}
