using System;

namespace Zlatmet2.Core
{
    public abstract class PersistentObject : BaseObject
    {
        protected PersistentObject(Guid id)
            : base(id)
        {
        }
    }
}
