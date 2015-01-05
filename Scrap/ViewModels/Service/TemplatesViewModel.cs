using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Scrap.Core.Classes.Service;
using Scrap.Models.Service;
using Scrap.ViewModels.Base;
using Scrap.Views.Service;
using Stimulsoft.Report;
using Xceed.Wpf.AvalonDock.Layout;

namespace Scrap.ViewModels.Service
{
    /// <summary>
    /// Модель представления вкладки "Шаблоны"
    /// </summary>
    public class TemplatesViewModel : BaseEditorViewModel<TemplateWrapper>
    {
        #region Поля

        private StiReport _report;

        private ICommand _showDesignerCommand;
        private ICommand _importCommand;
        private ICommand _exportCommand;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="optional"></param>
        public TemplatesViewModel(LayoutDocument layout, object optional = null)
            : base(layout, typeof(TemplatesView))
        {
            Title = "Шаблоны";

            Items.CollectionChanged += Items_CollectionChanged;

            foreach (Template template in MainStorage.Instance.TemplatesRepository.GetAll())
                Items.Add(new TemplateWrapper(template));

            if (Items.Any())
                SelectedItem = Items.First();
        }

        #region Свойства

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

        public ICommand ShowDesignerCommand
        {
            get { return _showDesignerCommand ?? (_showDesignerCommand = new RelayCommand(ShowDesigner)); }
        }

        public ICommand ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new RelayCommand(ImportTemplate)); }
        }

        public ICommand ExportCommand
        {
            get { return _exportCommand ?? (_exportCommand = new RelayCommand(ExportTemplate)); }
        }

        #endregion

        #region Методы

        public override void Dispose()
        {
            Items.CollectionChanged -= Items_CollectionChanged;

            foreach (var item in Items)
                item.PropertyChanged -= Template_PropertyChanged;

            base.Dispose();
        }

        protected override void AddItem()
        {
            TemplateWrapper templateWrapper = new TemplateWrapper { Data = new StiReport().SaveToByteArray() };
            Items.Add(templateWrapper);
            SelectedItem = templateWrapper;
        }

        protected override void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);

            switch (propertyName)
            {
                case "SelectedItem":
                    UpdatePreview();
                    break;
            }
        }

        private void UpdatePreview()
        {
            if (SelectedItem != null && SelectedItem.Data != null)
            {
                if (Report == null)
                    Report = new StiReport();

                try
                {
                    Report.Load(SelectedItem.Data);
                    Report.Compile();
                    Report.Render(false);
                }
                catch (Exception ex)
                {
                    string message = string.Format("Ошибка при загрузке шаблона{0}{1}", Environment.NewLine,
                        ex.Message);
                    MessageBox.Show(message, MainStorage.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (Report != null)
                {
                    Report.Dispose();
                    Report = null;
                }
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

            switch (e.PropertyName)
            {
                case "Data":
                    UpdatePreview();
                    break;
            }
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
            Report.Compile();
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

                MessageBox.Show("Импорт шаблона успешно завершён", MainStorage.AppName);
            }
        }

        private void ExportTemplate()
        {
            if (SelectedItem == null)
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Шаблоны отчётов (*.mrt)|*.mrt", FileName = SelectedItem.Name };
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllBytes(saveFileDialog.FileName, SelectedItem.Data);
        }

        #endregion

    }
}