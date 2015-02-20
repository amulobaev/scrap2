using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scrap.Core.Classes.Documents;
using Scrap.Core.Classes.References;
using Scrap.Models.Documents;
using Scrap.ViewModels.Base;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Documents
{
    /// <summary>
    /// Базовая модель представления для документов (перевозка, переработка, корректировка остатков)
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TItemWrapper"></typeparam>
    public abstract class BaseDocumentViewModel<TModel, TItemWrapper> : UniqueLayoutDocumentViewModel
        where TModel : BaseDocument
        where TItemWrapper : BaseDocumentItemWrapper
    {
        #region Поля

        private TItemWrapper _selectedItem;

        private int _number;

        private ICommand _saveAndCloseCommand;

        private ICommand _saveCommand;

        private ICommand _addItemCommand;

        private ICommand _removeItemCommand;

        private ICommand _moveItemUpCommand;

        private ICommand _moveItemDownCommand;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="viewType"></param>
        /// <param name="id"></param>
        protected BaseDocumentViewModel(LayoutDocument layout, Type viewType, Guid id)
            : base(layout, viewType, id)
        {
            Items = new ObservableCollection<TItemWrapper>();
            Items.CollectionChanged += ItemsOnCollectionChanged;
        }

        #region Свойства

        /// <summary>
        /// Номер документа
        /// </summary>
        public int Number
        {
            get { return _number; }
            set { Set(() => Number, ref _number, value); }
        }

        public ReadOnlyObservableCollection<Nomenclature> Nomenclatures
        {
            get { return MainStorage.Instance.Nomenclatures; }
        }

        public ObservableCollection<TItemWrapper> Items { get; private set; }

        public TItemWrapper SelectedItem
        {
            get { return _selectedItem; }
            set { Set(() => SelectedItem, ref _selectedItem, value); }
        }

        public TModel Container { get; protected set; }

        protected abstract string DocumentTitle { get; }

        #endregion

        #region Команды

        public ICommand SaveAndCloseCommand
        {
            get { return _saveAndCloseCommand ?? (_saveAndCloseCommand = new RelayCommand(SaveAndCloseDocument)); }
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveDocument)); }
        }

        public ICommand AddItemCommand
        {
            get { return _addItemCommand ?? (_addItemCommand = new RelayCommand(AddItem)); }
        }

        public ICommand RemoveItemCommand
        {
            get { return _removeItemCommand ?? (_removeItemCommand = new RelayCommand(RemoveItem)); }
        }

        public ICommand MoveItemUpCommand
        {
            get { return _moveItemUpCommand ?? (_moveItemUpCommand = new RelayCommand(MoveItemUp)); }
        }

        public ICommand MoveItemDownCommand
        {
            get { return _moveItemDownCommand ?? (_moveItemDownCommand = new RelayCommand(MoveItemDown)); }
        }

        #endregion

        #region Методы

        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            for (int i = 0; i < Items.Count; i++)
                Items[i].Number = i + 1;
        }

        protected abstract void SaveDocument();

        private void SaveAndCloseDocument()
        {
            SaveDocument();
            
            if (IsValid())
                Close();
        }

        protected abstract void AddItem();

        private void RemoveItem()
        {
            if (SelectedItem == null)
                return;

            if (MessageBox.Show("Действительно удалить?", MainStorage.AppName, MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Items.Remove(SelectedItem);
                UpdateNumbers();
            }
        }

        private void MoveItemUp()
        {
            if (Items.Count < 2)
                return;

            int index = Items.IndexOf(SelectedItem);
            if (index == 0)
                return;

            var temp = SelectedItem;

            Items.Remove(SelectedItem);
            Items.Insert(index - 1, temp);

            SelectedItem = temp;

            UpdateNumbers();
        }

        private void MoveItemDown()
        {
            if (Items.Count < 2)
                return;

            int index = Items.IndexOf(SelectedItem);
            if (index == Items.Count - 1)
                return;

            var temp = SelectedItem;

            Items.Remove(SelectedItem);
            Items.Insert(index + 1, temp);

            SelectedItem = temp;

            UpdateNumbers();
        }

        private void UpdateNumbers()
        {
            for (int i = 0; i < Items.Count; i++)
                Items[i].Number = i + 1;
        }

        protected abstract void UpdateTitle();

        /// <summary>
        /// Обновление журнала
        /// </summary>
        protected void UpdateJournal()
        {
            MainViewModel.Instance.UpdateJournal();
        }

        #endregion

    }
}
