using System;
using System.Diagnostics;

namespace Zlatmet2.Models.References
{
    public abstract class BaseReferenceWrapper : BaseWrapper
    {
        protected BaseReferenceWrapper(object dataForContainer = null)
            : base(dataForContainer)
        {
        }

        public Guid Id { get; protected set; }

        public virtual void Save()
        {
            if (!IsChanged)
                return;

            UpdateContainer();

            var ticks = Environment.TickCount;
            MainStorage.Instance.CreateOrUpdateObject(Container);
            Debug.WriteLine("CreateOrUpdateObject {0} мс", Environment.TickCount - ticks);

            IsChanged = false;
        }
    }
}
