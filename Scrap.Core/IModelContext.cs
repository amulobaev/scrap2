namespace Scrap.Core
{
    public interface IModelContext
    {
        string ConnectionString { get; }

        IConnectionFactory ConnectionFactory { get; }
    }
}