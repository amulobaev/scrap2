using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace Zlatmet2.ViewModels.Base
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CustomLayoutDocumentViewModel : LayoutDocumentViewModel
    {
        protected CustomLayoutDocumentViewModel(LayoutDocument layout, Type viewType, object optionalContent)
            : base(layout, viewType)
        {
            //this.OptionalContent = optionalContent;
        }

        public virtual object OptionalContent { get; set; }
    }
}