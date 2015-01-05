using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Scrap.Core.Classes.References;
using Scrap.Tools;

namespace Scrap.ViewModels.References
{
    public class ConvertContractorToDivisionViewModel : ViewModelBase
    {
        private readonly Guid _contractorId;
        private ICommand _okCommand;
        private ICommand _cancelCommand;
        private Organization _contractor;
        private readonly ObservableCollection<Organization> _contractors = new ObservableCollection<Organization>();
        private string _division;

        public ConvertContractorToDivisionViewModel(Guid contractorId)
        {
            _contractorId = contractorId;

            _contractors.AddRange(MainStorage.Instance.Contractors.Where(x => x.Id != contractorId));
        }

        public ObservableCollection<Organization> Contractors
        {
            get { return _contractors; }
        }

        public Organization Contractor
        {
            get { return _contractor; }
            set { Set(() => Contractor, ref _contractor, value); }
        }

        public string Division
        {
            get { return _division; }
            set { Set(() => Division, ref _division, value); }
        }

        public ICommand OkCommand
        {
            get { return _okCommand ?? (_okCommand = new RelayCommand<Window>(Ok)); }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand<Window>(Cancel)); }
        }

        private void Ok(Window window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            if (Contractor == null)
            {
                MessageBox.Show("Не выбран контрагент", MainStorage.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(Division))
            {
                MessageBox.Show("Не указано наименование подразделения", MainStorage.AppName, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (Contractor.Divisions.Any(x => x.Name == Division))
            {
                string message = string.Format("У контрагента \"{0}\" уже есть подразделение \"{1}\"", Contractor.Name,
                    Division);
                MessageBox.Show(message, MainStorage.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            window.DialogResult = true;
        }

        private void Cancel(Window window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            window.DialogResult = false;
        }
    }
}
