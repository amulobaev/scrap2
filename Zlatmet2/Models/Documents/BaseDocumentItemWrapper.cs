using System;

namespace Zlatmet2.Models.Documents
{
    public abstract class BaseDocumentItemWrapper : BaseValidationWrapper
    {
        private int _number;

        protected BaseDocumentItemWrapper(object dataForContainer = null)
            : base(dataForContainer)
        {
        }

        public Guid Id { get; protected set; }

        public int Number
        {
            get { return _number; }
            set
            {
                if (value == _number)
                    return;
                _number = value;
                RaisePropertyChanged("Number");
            }
        }
    }
}
