using System;

namespace Zlatmet2.Core
{
    public abstract class PersistentObject : BaseObject
    {
        //protected PersistentObject(IModelContext context, Guid id)
        protected PersistentObject(Guid id)
            : base(id)
        {
        }
    }
}
