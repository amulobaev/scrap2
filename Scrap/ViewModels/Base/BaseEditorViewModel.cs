﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scrap.Models;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Base
{
    /// <summary>
    /// Базовая модель представления для табличного редактора (справочники, пользователи)
    /// </summary>
    public abstract class BaseEditorViewModel<T> : SingletonLayoutDocumentViewModel where T : BaseReferenceWrapper
    {
        private T _selectedItem;

        private ICommand _addItemCommand;

        private ICommand _removeItemCommand;

        private ICommand _saveCommand;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="viewType"></param>
        protected BaseEditorViewModel(LayoutDocument layout, Type viewType)
            : base(layout, viewType)
        {
            Items = new ObservableCollection<T>();
        }

        public ObservableCollection<T> Items { get; private set; }

        public T SelectedItem
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

        public ICommand AddItemCommand
        {
            get { return _addItemCommand ?? (_addItemCommand = new RelayCommand(AddItem)); }
        }

        public ICommand RemoveItemCommand
        {
            get { return _removeItemCommand ?? (_removeItemCommand = new RelayCommand(RemoveItem)); }
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveChanges)); }
        }

        protected abstract void AddItem();

        private void RemoveItem()
        {
            if (SelectedItem == null)
                return;

            if (MessageBox.Show("Действительно удалить?", MainStorage.AppName, MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (SelectedItem.Container != null)
                    MainStorage.Instance.DeleteObject(SelectedItem.Container);
                Items.Remove(SelectedItem);
            }
        }

        protected void SaveChanges()
        {
            foreach (var item in Items.Where(x => x.IsChanged))
                item.Save();
        }

    }
}
