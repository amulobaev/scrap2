using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace Zlatmet2.ViewModels.Base
{
    public abstract class UniqueLayoutDocumentViewModel : LayoutDocumentViewModel
    {
        protected UniqueLayoutDocumentViewModel(LayoutDocument layout, Type viewType, Guid id)
            : base(layout, viewType)
        {
            Id = id;
        }

        public Guid Id { get; protected set; }

    }
}
