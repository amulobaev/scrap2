using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Models.References;
using Zlatmet2.Views.References;

namespace Zlatmet2.ViewModels.References
{
    public abstract class ReferenceOrganizationsViewModel : BaseReferenceViewModel<OrganizationWrapper>
    {
        private ICommand _addDivisionCommand;
        private ICommand _removeDivisionCommand;
        private ICommand _moveDivisionUpCommand;
        private ICommand _moveDivisionDownCommand;
        private DivisionWrapper _selectedDivision;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        protected ReferenceOrganizationsViewModel(LayoutDocument layout)
            : base(layout, typeof(ReferenceOrganizationsView))
        {
        }

        #region Свойства

        public ICommand AddDivisionCommand
        {
            get { return _addDivisionCommand ?? (_addDivisionCommand = new RelayCommand(AddDivision)); }
        }

        public ICommand RemoveDivisionCommand
        {
            get { return _removeDivisionCommand ?? (_removeDivisionCommand = new RelayCommand(RemoveDivision)); }
        }

        public ICommand MoveDivisionUpCommand
        {
            get { return _moveDivisionUpCommand ?? (_moveDivisionUpCommand = new RelayCommand(MoveDivisionUp)); }
        }

        public ICommand MoveDivisionDownCommand
        {
            get { return _moveDivisionDownCommand ?? (_moveDivisionDownCommand = new RelayCommand(MoveDivisionDown)); }
        }

        #endregion

        public DivisionWrapper SelectedDivision
        {
            get { return _selectedDivision; }
            set
            {
                if (Equals(value, _selectedDivision))
                    return;
                _selectedDivision = value;
                RaisePropertyChanged("SelectedDivision");
            }
        }

        private void AddDivision()
        {
            var newDivision = new DivisionWrapper(SelectedItem.Id, SelectedItem.Divisions.Count + 1, "Новое подразделение");
            SelectedItem.Divisions.Add(newDivision);
            SelectedDivision = newDivision;
            SelectedItem.IsChanged = true;
        }

        private void RemoveDivision()
        {
            if (SelectedItem.Divisions.Count < 2)
            {
                MessageBox.Show("У организации должно быть как минимум одно подразделение");
                return;
            }

            if (MessageBox.Show("Действительно удалить?", MainStorage.AppName, MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
                SelectedItem.Divisions.Remove(SelectedDivision);

            UpdateNumbersInDivisions();
        }

        private void MoveDivisionUp()
        {
            if (SelectedItem.Divisions.Count < 2)
                return;

            int index = SelectedItem.Divisions.IndexOf(SelectedDivision);
            if (index == 0)
                return;

            var temp = SelectedDivision;

            SelectedItem.Divisions.Remove(SelectedDivision);
            SelectedItem.Divisions.Insert(index - 1, temp);

            SelectedDivision = temp;

            UpdateNumbersInDivisions();
        }

        private void MoveDivisionDown()
        {
            if (SelectedItem.Divisions.Count < 2)
                return;

            int index = SelectedItem.Divisions.IndexOf(SelectedDivision);
            if (index == SelectedItem.Divisions.Count - 1)
                return;

            var temp = SelectedDivision;

            SelectedItem.Divisions.Remove(SelectedDivision);
            SelectedItem.Divisions.Insert(index + 1, temp);

            SelectedDivision = temp;

            UpdateNumbersInDivisions();
        }

        private void UpdateNumbersInDivisions()
        {
            if (SelectedItem == null)
                return;

            for (int i = 0; i < SelectedItem.Divisions.Count; i++)
                SelectedItem.Divisions[i].Number = i + 1;
        }

    }
}
