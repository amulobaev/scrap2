using System;

namespace Scrap.Core
{
    public abstract class BaseObject
    {
        private readonly Guid _id;

        protected BaseObject(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Идентификатор объекта должен быть уникальным", "id");
            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}
