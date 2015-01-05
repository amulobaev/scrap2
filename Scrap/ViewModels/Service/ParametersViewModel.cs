using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scrap.ViewModels.Base;

namespace Scrap.ViewModels.Service
{
    public sealed class ParametersViewModel : ViewModelBase
    {
        private bool _showJournal;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ParametersViewModel()
        {
            OkCommand = new RelayCommand<Window>(Ok);
            CancelCommand = new RelayCommand<Window>(Cancel);

            ShowJournal = MainStorage.Instance.ShowJournal;
        }

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public bool ShowJournal
        {
            get { return _showJournal; }
            set
            {
                if (value.Equals(_showJournal)) return;
                _showJournal = value;
                RaisePropertyChanged("ShowJournal");
            }
        }

        private void Ok(object parameter)
        {
            Window window = parameter as Window;
            if (window == null)
                return;

            MainStorage.Instance.ShowJournal = ShowJournal;
            MainStorage.Instance.SaveSettings();

            window.DialogResult = true;
        }

        private void Cancel(object parameter)
        {
            Window window = parameter as Window;
            if (window == null)
                return;

            window.DialogResult = false;
        }

    }
}
