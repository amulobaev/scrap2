using System;

namespace Scrap.Models
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
            MainStorage.Instance.CreateOrUpdateObject(Container);
            IsChanged = false;
        }
    }
}
