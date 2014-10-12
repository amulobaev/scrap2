using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Enums;
using Zlatmet2.Models.References;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.ViewModels.Documents;
using Zlatmet2.Views.References;

namespace Zlatmet2.ViewModels.References
{
    /// <summary>
    /// Модель представления справочника "Контрагенты"
    /// </summary>
    public sealed class ReferenceContractorsViewModel : BaseEditorViewModel<OrganizationWrapper>
    {
        private ICommand _addDivisionCommand;
        private ICommand _removeDivisionCommand;
        private ICommand _moveDivisionUpCommand;
        private ICommand _moveDivisionDownCommand;
        private DivisionWrapper _selectedDivision;
        private ICommand _convertToDivisionCommand;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="optional"></param>
        public ReferenceContractorsViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(ReferenceContractorsView))
        {
            Title = "Справочник: контрагенты";

            foreach (var supplier in MainStorage.Instance.Contractors)
                Items.Add(new OrganizationWrapper(supplier));
        }

        #region Свойства

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

        #endregion

        #region Команды

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

        public ICommand ConvertToDivisionCommand
        {
            get
            {
                return _convertToDivisionCommand ?? (_convertToDivisionCommand = new RelayCommand(ConvertToDivision));
            }
        }

        #endregion

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

            SelectedItem.IsChanged = true;

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

        protected override void AddItem()
        {
            OrganizationWrapper supplier = new OrganizationWrapper(OrganizationType.Contractor, "Новый контрагент");
            supplier.Divisions.Add(new DivisionWrapper(supplier.Id, 1, "Основное"));
            Items.Add(supplier);
            SelectedItem = supplier;
        }

        private void ConvertToDivision()
        {
            if (SelectedItem == null)
                return;

            // Проверка количества подразделений у контрагента
            if (SelectedItem.Divisions.Count > 1)
            {
                MessageBox.Show("У контрагента для преобразования может быть только одно подразделение", MainStorage.AppName, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            // Попросить закрыть все документы "Перевозка"
            if (MainViewModel.Instance.Documents.Any(
                    x =>
                        x.Content is FrameworkElement &&
                        (x.Content as FrameworkElement).DataContext is DocumentTransportationViewModel))
            {
                MessageBox.Show("Закройте все документы \"Перевозка\"", MainStorage.AppName, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            //
            ConvertContractorToDivisionWindow window = new ConvertContractorToDivisionWindow(SelectedItem.Id)
            {
                Owner = MainWindow.Instance
            };
            if (window.ShowDialog() == true)
            {
                // Нужно создать подразделение у целевого контрагента
                OrganizationWrapper newContractorWrapper = Items.FirstOrDefault(x => x.Id == window.ViewModel.Contractor.Id);
                if (newContractorWrapper == null)
                    return;
                DivisionWrapper newDivisionWrapper = new DivisionWrapper(newContractorWrapper.Id,
                    newContractorWrapper.Divisions.Count + 1, window.ViewModel.Division);
                newContractorWrapper.Divisions.Add(newDivisionWrapper);
                newContractorWrapper.IsChanged = true;

                // Сохранение изменений
                SaveChanges();

                // Замена контрагента в документах
                MainStorage.Instance.TransportationRepository.ConvertContractorToDivision(SelectedItem.Id,
                    SelectedItem.Divisions[0].Id, newContractorWrapper.Id, newDivisionWrapper.Id);

                // Удаление контрагента
                MainStorage.Instance.DeleteObject(SelectedItem.Container);
                Items.Remove(SelectedItem);

                SelectedItem = newContractorWrapper;
            }
        }

    }
}
