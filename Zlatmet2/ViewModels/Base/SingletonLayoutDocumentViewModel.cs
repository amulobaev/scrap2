using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace Zlatmet2.ViewModels.Base
{
    public abstract class SingletonLayoutDocumentViewModel : LayoutDocumentViewModel
    {
        protected SingletonLayoutDocumentViewModel(LayoutDocument layout, Type viewType)
            : base(layout, viewType)
        {
        }
    }
}
