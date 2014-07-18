using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace Zlatmet2.ViewModels.Base
{
    /// <summary>
    /// Базовая модель представления для вкладки в DocumentHost
    /// </summary>
    public abstract class LayoutDocumentViewModel : LayoutContentViewModel
    {
        protected LayoutDocumentViewModel(LayoutDocument layout, Type viewType)
            : base(layout, viewType)
        {
        }

        public new LayoutDocument Layout
        {
            get { return (LayoutDocument)base.Layout; }
            protected set { base.Layout = value; }
        }
    }
}