using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;
using Zlatmet2.Models.Service;
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
        private ICommand _showDesignerCommand;
        private StiReport _report;

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
            this.PropertyChanged += OnPropertyChanged;

            foreach (var template in MainStorage.Instance.TemplatesRepository.GetAll())
                Items.Add(new TemplateWrapper(template));

            if (Items.Any())
                SelectedItem = Items.First();
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

        public StiReport Report
        {
            get { return _report; }
            set
            {
                if (Equals(value, _report))
                    return;
                _report = value;
                RaisePropertyChanged("Report");
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

        public ICommand ShowDesignerCommand
        {
            get { return _showDesignerCommand ?? (_showDesignerCommand = new RelayCommand(ShowDesigner)); }
        }

        #endregion

        #region Методы

        public override void Dispose()
        {
            Items.CollectionChanged -= Items_CollectionChanged;
            this.PropertyChanged -= OnPropertyChanged;

            foreach (var item in Items)
                item.PropertyChanged -= Template_PropertyChanged;
        }

        /// <summary>
        /// Обработка события изменения свойства модели представления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedItem":
                    if (SelectedItem != null && SelectedItem.Data != null)
                    {
                        if (Report == null)
                            Report = new StiReport();
                        Report.Load(SelectedItem.Data);
                        Report.Render(false);
                    }
                    else
                    {
                        if (Report != null)
                        {
                            Report.Dispose();
                            Report = null;
                        }
                    }
                    break;
            }
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

            // Если у обёртки шаблона изменилось какое-либо свойство, то изменения нужно сохранить в базу
            templateWrapper.Save();

            switch (e.PropertyName)
            {
                case "Data":
                    // Если изменились данные выбранного шаблона то его нужно перерисовать
                    //if (templateWrapper == SelectedItem)
                    //{
                    //    if (Report == null)
                    //        Report = new StiReport();
                    //    Report.Load(templateWrapper.Data);
                    //    Report.Render(false);
                    //}
                    break;
            }
        }

        private void AddTemplate()
        {
            TemplateWrapper templateWrapper = new TemplateWrapper { Data = new StiReport().SaveToByteArray() };
            Items.Add(templateWrapper);
            SelectedItem = templateWrapper;
        }

        private void DeleteTemplate()
        {
            if (SelectedItem == null)
                return;

            if (MessageBox.Show("Действительно удалить?", MainStorage.AppName, MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                Items.Remove(SelectedItem);
        }

        private void ShowDesigner()
        {
            if (SelectedItem == null)
                return;

            TemplateEditorWindow window = new TemplateEditorWindow(SelectedItem) { Owner = MainWindow.Instance };
            window.ShowDialog();

            if (Report == null)
                Report = new StiReport();
            Report.Load(SelectedItem.Data);
            Report.Render(false);
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