using System;
using Scrap.Core;

namespace Scrap.Domain.Repositories
{
    /// <summary>
    /// Базовый репозитарий
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly IModelContext Context;
        protected readonly IConnectionFactory ConnectionFactory;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        protected BaseRepository(IModelContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            Context = context;
            ConnectionFactory = context.ConnectionFactory;
        }

    }
}
