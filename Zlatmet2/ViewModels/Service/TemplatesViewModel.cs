using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Models.Service;
using Zlatmet2.Tools;
using Zlatmet2.ViewModels.Base;
using Zlatmet2.Views.Service;

namespace Zlatmet2.ViewModels.Service
{
    /// <summary>
    /// Модель представления вкладки "Шаблоны"
    /// </summary>
    public class TemplatesViewModel : SingletonLayoutDocumentViewModel
    {
        #region Поля

        private readonly ObservableCollection<TemplateWrapper> _items = new ObservableCollection<TemplateWrapper>();
        private TemplateWrapper _selectedItem;

        private ICommand _addCommand;
        private ICommand _deleteCommand;
        private ICommand _importCommand;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        public TemplatesViewModel(LayoutDocument layout)
            : base(layout, typeof(TemplatesView))
        {
            Title = "Шаблоны";

            Items.CollectionChanged += Items_CollectionChanged;

            foreach (var template in MainStorage.Instance.TemplatesRepository.GetAll())
                Items.Add(new TemplateWrapper(template));
        }

        #region Свойства

        public ObservableCollection<TemplateWrapper> Items
        {
            get { return _items; }
        }

        public TemplateWrapper SelectedItem
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

        #endregion

        #region Команды

        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(AddTemplate)); }
        }

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(DeleteTemplate)); }
        }

        public ICommand ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new RelayCommand(ImportTemplate)); }
        }

        #endregion

        #region Методы

        public override void Dispose()
        {
            foreach (var item in Items)
                item.PropertyChanged -= Template_PropertyChanged;

            Items.CollectionChanged -= Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //
            if (e.NewItems != null)
            {
                foreach (TemplateWrapper newItem in e.NewItems)
                {
                    newItem.PropertyChanged += Template_PropertyChanged;
                    newItem.Save();
                }
            }

            //
            if (e.OldItems != null)
            {
                foreach (TemplateWrapper oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= Template_PropertyChanged;
                    if (oldItem.Container != null)
                        MainStorage.Instance.TemplatesRepository.Delete(oldItem.Container);
                }
            }
        }

        private void Template_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TemplateWrapper templateWrapper = sender as TemplateWrapper;
            if (templateWrapper == null)
                return;

            templateWrapper.Save();
        }

        private void AddTemplate()
        {
            TemplateWrapper template = new TemplateWrapper();
            Items.Add(template);
        }

        private void DeleteTemplate()
        {
            if (SelectedItem == null)
                return;

            Items.Remove(SelectedItem);
        }

        private void ImportTemplate()
        {
            if (SelectedItem == null)
                return;

            var openFileDialog = new OpenFileDialog { Filter = "Шаблоны отчётов (*.mrt)|*.mrt" };
            if (openFileDialog.ShowDialog() == true)
            {
                var fileInfo = new FileInfo(openFileDialog.FileName);

                if (fileInfo.Length == 0)
                {
                    MessageBox.Show("Файл шаблона пустой");
                    return;
                }

                using (var binaryReader = new BinaryReader(fileInfo.OpenRead()))
                    SelectedItem.Data = binaryReader.ReadBytes((int)fileInfo.Length);
            }
        }

        #endregion

    }
}