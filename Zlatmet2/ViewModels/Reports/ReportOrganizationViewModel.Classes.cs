using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Zlatmet2.Core.Classes.References;

namespace Zlatmet2.ViewModels.Reports
{
    public partial class ReportOrganizationViewModel
    {
        public class BaseTreeItem : ObservableObject
        {
            private bool _isChecked;

            private string _name;

            public bool IsChecked
            {
                get { return _isChecked; }
                set
                {
                    if (value == _isChecked)
                        return;
                    _isChecked = value;
                    RaisePropertyChanged(() => IsChecked);
                }
            }

            public string Name
            {
                get { return _name; }
                set
                {
                    if (value == _name)
                        return;
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
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
            }

            public Organization Contractor
            {
                get { return _contractor; }
            }

            public ObservableCollection<DivisionWrapper> Divisions
            {
                get { return _divisions; }
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
            }

            public Division Division
            {
                get { return _division; }
            }
        }

    }
}
