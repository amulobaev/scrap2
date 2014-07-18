using System;
using Zlatmet2.Core;

namespace Zlatmet2.Domain.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IModelContext Context;
        protected readonly IConnectionFactory ConnectionFactory;

        protected BaseRepository(IModelContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            Context = context;
            ConnectionFactory = context.ConnectionFactory;
        }

    }
}
