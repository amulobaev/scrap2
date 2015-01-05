using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Base
{
    public abstract class SingletonLayoutDocumentViewModel : CustomLayoutDocumentViewModel
    {
        protected SingletonLayoutDocumentViewModel(LayoutDocument layout, Type viewType, object dataForContainer = null)
            : base(layout, viewType, dataForContainer)
        {
        }
    }
}
