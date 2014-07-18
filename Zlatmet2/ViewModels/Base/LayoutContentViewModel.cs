using System;
using System.ComponentModel;
using Xceed.Wpf.AvalonDock.Layout;

namespace Zlatmet2.ViewModels.Base
{
    /// <summary>
    /// Базовая модель представления для вкладок
    /// </summary>
    public abstract class LayoutContentViewModel : BaseValidationViewModel, IDisposable
    {
        private string _title;

        protected LayoutContentViewModel(LayoutContent layout, Type viewType)
        {
            if (layout == null)
                throw new ArgumentNullException("layout");
            Layout = layout;

            if (viewType == null)
                throw new ArgumentNullException("viewType");
            ViewType = viewType;

            this.PropertyChanged += OnPropertyChanged;

            layout.Closed += LayoutOnClosed;
            layout.Closing += (sender, args) => RaiseClosing(args);
        }

        public LayoutContent Layout { get; protected set; }

        public Type ViewType { get; private set; }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                    return;
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        public event EventHandler Closed;

        public event EventHandler<CancelEventArgs> Closing;

        private void LayoutOnClosed(object sender, EventArgs args)
        {
            RaiseClosed();

            this.Dispose();
        }

        private void RaiseClosed()
        {
            EventHandler handler = Closed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void RaiseClosing(CancelEventArgs e)
        {
            EventHandler<CancelEventArgs> handler = Closing;
            if (handler != null)
                handler(this, e);
        }

        public virtual bool CanClose()
        {
            return true;
        }

        public virtual void Dispose()
        {
            this.PropertyChanged -= OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Title":
                    Layout.Title = Title;
                    break;
            }
        }

    }
}
