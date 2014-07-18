using System;

namespace Zlatmet2.Core
{
    public abstract class BaseObject
    {
        //protected readonly IModelContext Context;

        private readonly Guid _id;

        //protected BaseObject(IModelContext context, Guid id)
        protected BaseObject(Guid id)
        {
            //if (context == null)
            //    throw new ArgumentNullException("context");
            if (id == Guid.Empty)
                throw new ArgumentException("Идентификатор объекта должен быть уникальным", "id");

            //Context = context;
            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}
