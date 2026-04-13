namespace HealthSync.BuildingBlocks.Abstraction.Contexts.Message
{
    public interface IMessageBody
    {
        byte[] GetBytes();

        string GetString();

        long? Length { get; }
    }
}
