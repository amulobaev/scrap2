using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Core.Classes.Documents;
using Zlatmet2.Core.Classes.References;
using Zlatmet2.Models.Documents;
using Zlatmet2.ViewModels.Base;

namespace Zlatmet2.ViewModels.Documents
{
    public abstract class BaseDocumentViewModel<TModel, TItemWrapper> : UniqueLayoutDocumentViewModel
        where TModel : BaseDocument
        where TItemWrapper : BaseDocumentItemWrapper
    {
        #region Поля

        private TItemWrapper _selectedItem;

        private int _number;

        private DateTime? _date;

        private ICommand _saveDocumentCommand;

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

        /// <summary>
        /// Номер документа
        /// </summary>
        public int Number
        {
            get { return _number; }
            set
            {
                if (value == _number)
                    return;
                _number = value;
                RaisePropertyChanged("Number");
            }
        }

        /// <summary>
        /// Дата документа
        /// </summary>
        [Required]
        public DateTime? Date
        {
            get { return _date; }
            set
            {
                if (value.Equals(_date))
                    return;
                _date = value;
                RaisePropertyChanged("Date");
            }
        }

        public ReadOnlyObservableCollection<Nomenclature> Nomenclatures
        {
            get { return MainStorage.Instance.Nomenclatures; }
        }

        public ObservableCollection<TItemWrapper> Items { get; private set; }

        public TItemWrapper SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem))
                    return;
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        public TModel Document { get; protected set; }

        #region Команды

        public ICommand SaveDocumentCommand
        {
            get { return _saveDocumentCommand ?? (_saveDocumentCommand = new RelayCommand(SaveDocument)); }
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

        #endregion

    }
}
