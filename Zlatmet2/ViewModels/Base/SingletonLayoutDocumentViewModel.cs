using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace Zlatmet2.ViewModels.Base
{
    public abstract class SingletonLayoutDocumentViewModel : CustomLayoutDocumentViewModel
    {
        protected SingletonLayoutDocumentViewModel(LayoutDocument layout, Type viewType, object dataForContainer = null)
            : base(layout, viewType, dataForContainer)
        {
        }
    }
}
