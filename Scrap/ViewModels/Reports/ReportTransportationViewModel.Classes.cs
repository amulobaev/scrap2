﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Scrap.Core.Classes.References;

namespace Scrap.ViewModels.Reports
{
    public partial class ReportTransportationViewModel
    {
        public abstract class BaseTreeItem : ObservableObject
        {
            private readonly Guid _id;
            private bool _isChecked;

            private string _name;

            protected BaseTreeItem(Guid id, string name)
            {
                _id = id;
                Name = name;
            }

            public Guid Id
            {
                get { return _id; }
            }

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
                : base(contractor.Id, contractor.Name)
            {
                _contractor = contractor;

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
                : base(division.Id, division.Name)
            {
                _contractorWrapper = contractorWrapper;
                _division = division;

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
