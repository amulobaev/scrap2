namespace Zlatmet2.Core
{
    public interface IModelContext
    {
        string ConnectionString { get; }

        IConnectionFactory ConnectionFactory { get; }
    }
}