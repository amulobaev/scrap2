using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Zlatmet2.Core.Classes.References;

namespace Zlatmet2.ViewModels.Reports
{
    public partial class ReportTransportationViewModel
    {
        public class BaseTreeItem : ObservableObject
        {
            private bool _isChecked;

            private string _name;

            public bool IsChecked
            {
                get { return _isChecked; }
                set { Set(() => IsChecked, ref _isChecked, value); }
            }

            public string Name
            {
                get { return _name; }
                set { Set(() => Name, ref _name, value); }
            }
        }

        /// <summary>
        /// Обёртка для контрагента
        /// </summary>
        public class ContractorWrapper : BaseTreeItem
        {
            private readonly Organization _contractor;

            private readonly ObservableCollection<DivisionWrapper> _divisions =
                new ObservableCollection<DivisionWrapper>();

            public ContractorWrapper(Organization contractor)
            {
                _contractor = contractor;

                Name = contractor.Name;

                foreach (Division division in contractor.Divisions.OrderBy(x => x.Number))
                    Divisions.Add(new DivisionWrapper(this, division));

                this.PropertyChanged += OnPropertyChanged;
            }

            public Organization Contractor
            {
                get { return _contractor; }
            }

            public ObservableCollection<DivisionWrapper> Divisions
            {
                get { return _divisions; }
            }

            private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case "IsChecked":
                        foreach (var division in Divisions)
                            division.IsChecked = IsChecked;
                        break;
                }
            }

        }

        /// <summary>
        /// Обёртка для подразделения контрагента
        /// </summary>
        public class DivisionWrapper : BaseTreeItem
        {
            private readonly ContractorWrapper _contractorWrapper;

            private readonly Division _division;

            public DivisionWrapper(ContractorWrapper contractorWrapper, Division division)
            {
                _contractorWrapper = contractorWrapper;
                _division = division;

                Name = division.Name;

                this.PropertyChanged += OnPropertyChanged;
            }

            private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case "IsChecked":
                        if (_contractorWrapper.Divisions.All(x => x.IsChecked) && !_contractorWrapper.IsChecked)
                            _contractorWrapper.IsChecked = true;
                        else if (_contractorWrapper.Divisions.All(x => !x.IsChecked) && _contractorWrapper.IsChecked)
                            _contractorWrapper.IsChecked = false;
                        break;
                }
            }

            public Division Division
            {
                get { return _division; }
            }
        }
    }
}
