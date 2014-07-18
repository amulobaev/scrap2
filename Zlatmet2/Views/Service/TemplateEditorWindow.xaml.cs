using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Stimulsoft.Report;
using Stimulsoft.Report.Design;
using Zlatmet2.Core.Classes.Service;

namespace Zlatmet2.Views.Service
{
    /// <summary>
    /// Логика взаимодействия для TemplateEditorWindow.xaml
    /// </summary>
    public partial class TemplateEditorWindow : Window
    {
        private readonly Guid _templateId;
        private readonly Template _template;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="templateId">Идентификатор шаблона</param>
        public TemplateEditorWindow(Guid templateId)
        {
            InitializeComponent();

            _templateId = templateId;

            StiOptions.Engine.GlobalEvents.SavingReportInDesigner += GlobalEvents_SavingReportInDesigner;

            _template = MainStorage.Instance.TemplatesRepository.GetById(templateId);

        }

        private void GlobalEvents_SavingReportInDesigner(object sender, StiSavingObjectEventArgs e)
        {
            
        }

    }
}
