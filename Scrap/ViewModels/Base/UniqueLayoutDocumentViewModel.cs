using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Base
{
    public abstract class UniqueLayoutDocumentViewModel : CustomLayoutDocumentViewModel
    {
        protected UniqueLayoutDocumentViewModel(LayoutDocument layout, Type viewType, Guid id, object dataForContainer = null)
            : base(layout, viewType, dataForContainer)
        {
            Id = id;
        }

        public Guid Id { get; protected set; }
    }
}
