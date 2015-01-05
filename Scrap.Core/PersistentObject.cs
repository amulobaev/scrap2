using System;

namespace Scrap.Core
{
    public abstract class PersistentObject : BaseObject
    {
        protected PersistentObject(Guid id)
            : base(id)
        {
        }
    }
}
