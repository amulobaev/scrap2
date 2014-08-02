using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace Zlatmet2.ViewModels.Base
{
    public abstract class CustomLayoutDocumentViewModel : LayoutDocumentViewModel
    {
        protected CustomLayoutDocumentViewModel(LayoutDocument layout, Type viewType, object dataForContainer)
            : base(layout, viewType)
        {
            Container = dataForContainer;
        }

        public object Container { get; protected set; }

        public virtual void SetContainer(object dataForContainer)
        { }
    }
}